using System.Text.Json;
using Confluent.Kafka;
using Contract.Common;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka;

/// <summary>
/// Потребляет Kafka сообщения
/// </summary>
/// <param name="scopeFactory"></param>
/// <param name="options"></param>
/// <param name="logger"></param>
public class KafkaConsumer(
    IServiceScopeFactory scopeFactory,
    IOptions<KafkaOptions> options,
    ILogger<KafkaConsumer> logger)
    : BackgroundService
{
    private readonly KafkaOptions _options = options.Value;

    private const string ConsumerName = "UsersConsumer";

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,

            // Имя группы потребителей
            GroupId = _options.GroupId,

            // Если группа новая и Offset отсутствует, читать с самого начала Topic
            AutoOffsetReset = AutoOffsetReset.Earliest,

            // Убрать авто коммиты
            EnableAutoCommit = false
        };

        //using var consumer = new ConsumerBuilder<string, string>(config).Build();
        using var consumer =
            new ConsumerBuilder<string, string>(config)
                .SetPartitionsAssignedHandler((_, partitions) =>
                {
                    logger.LogInformation(
                        "Assigned partitions: {Partitions}", string.Join(", ", partitions));
                })
                .Build();
        // подписываемся на topic
        consumer.Subscribe("users-topic");

        logger.LogInformation("Kafka Consumer started");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = consumer.Consume(cancellationToken);

                using var scope = scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<CaDemoDbContext>();

                IntegrationEventEnvelope? envelope;

                try
                {
                    envelope = JsonSerializer.Deserialize<IntegrationEventEnvelope>(result.Message.Value);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to deserialize IntegrationEventEnvelope");

                    consumer.Commit(result);

                    continue;
                }

                if (envelope is null)
                {
                    logger.LogWarning("Received empty IntegrationEventEnvelope");

                    consumer.Commit(result);

                    continue;
                }

                // IDEMPOTENCY CHECK
                var alreadyProcessed = await dbContext.InboxMessages.AnyAsync(x =>
                        x.EventId == envelope.EventId && x.ConsumerName == ConsumerName,
                    cancellationToken);

                if (alreadyProcessed)
                {
                    LogMessage("Duplicate event skipped", envelope.EventId, ConsumerName);

                    consumer.Commit(result);

                    continue;
                }

                var inboxMessage = InboxMessage.Create(envelope.EventId, ConsumerName);

                dbContext.InboxMessages.Add(inboxMessage);

                await dbContext.SaveChangesAsync(cancellationToken);

                try
                {
                    LogMessageReceived(envelope);

                    await ProcessMessageAsync(envelope, cancellationToken);

                    inboxMessage.MarkCompleted();

                    await dbContext.SaveChangesAsync(cancellationToken);

                    consumer.Commit(result);

                    LogMessage("Message processed", envelope.EventId, ConsumerName);
                }
                catch (Exception ex)
                {
                    LogMessage("Failed processing message", envelope.EventId, ConsumerName);

                    inboxMessage.MarkFailed(ex.Message);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    consumer.Commit(result);
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Kafka Consumer stopped");
        }
        catch (ConsumeException e)
        {
            logger.LogCritical(e, "Kafka Consumer error");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Kafka Consumer syntax error");
        }
        finally
        {
            consumer.Close();
        }
    }

    private static async Task ProcessMessageAsync(IntegrationEventEnvelope envelope,
        CancellationToken cancellationToken)
    {
        await Task.Delay(
            1000,
            cancellationToken);
    }

    private void LogMessageReceived(IntegrationEventEnvelope envelope)
    {
        logger.LogInformation(
            """
            Event received

            EventId: {EventId}
            EventType: {EventType}
            OccurredOnUtc: {OccurredOnUtc}
            """,
            envelope.EventId,
            envelope.EventType,
            envelope.OccurredOnUtc);
    }

    private void LogMessage(string message, Guid eventId, string consumer)
    {
        logger.LogInformation(
            """
            {message}

            EventId: {eventId}
            Consumer: {consumer}
            """,
            message,
            eventId,
            consumer);
    }
}
