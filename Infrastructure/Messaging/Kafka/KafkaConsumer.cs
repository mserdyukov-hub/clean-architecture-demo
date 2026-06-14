using Confluent.Kafka;
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

                var topic = result.Topic;
                var partition = result.Partition.Value;
                var offset = result.Offset.Value;

                var alreadyProcessed = await dbContext.InboxMessages.AnyAsync(x =>
                        x.Topic == topic && x.Partition == partition && x.Offset == offset,
                    cancellationToken);

                if (alreadyProcessed)
                {
                    LogMessage("Duplicate message skipped", topic, partition, offset);

                    consumer.Commit(result);

                    continue;
                }

                try
                {
                    LogMessageReceived(result);

                    await ProcessMessageAsync(result, cancellationToken);

                    var inboxMessage = InboxMessage.Create(topic, partition, offset);

                    inboxMessage.MarkProcessed();

                    dbContext.InboxMessages.Add(
                        inboxMessage);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    LogMessage("Message processed", topic, partition, offset);
                }
                catch (Exception ex)
                {
                    LogMessage("Failed processing message", topic, partition, offset);
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

    private static async Task ProcessMessageAsync(
        ConsumeResult<string, string> result,
        CancellationToken cancellationToken)
    {
        await Task.Delay(
            1000,
            cancellationToken);

        // Здесь будет настоящая бизнес-логика
    }

    private void LogMessageReceived(
        ConsumeResult<string, string> result)
    {
        logger.LogInformation(
            """
            Message received

            Topic: {Topic}
            Partition: {Partition}
            Offset: {Offset}
            Value: {Value}
            """,
            result.Topic,
            result.Partition.Value,
            result.Offset.Value,
            result.Message.Value);
    }

    private void LogMessage(
        string message,
        string topic,
        int partition,
        long offset)
    {
        logger.LogInformation(
            """
            {message}

            Topic: {Topic}
            Partition: {Partition}
            Offset: {Offset}
            """,
            message,
            topic,
            partition,
            offset);
    }
}
