using Application.Common.Messaging;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices;

/// <summary>
/// Публикует Outbox в Kafka
/// </summary>
/// <param name="scopeFactory"></param>
/// <param name="kafkaProducer"></param>
/// <param name="logger"></param>
public sealed class OutboxPublisher(
    IServiceScopeFactory scopeFactory,
    IKafkaProducer kafkaProducer,
    ILogger<OutboxPublisher> logger)
    : BackgroundService
{
    private const int MaxRetryCount = 5;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<CaDemoDbContext>();

                var messages = await dbContext.Set<OutboxMessage>()
                    .Where(x => x.ProcessedOnUtc == null && x.RetryCount < MaxRetryCount)
                    .OrderBy(x => x.ProcessedOnUtc)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                if (messages.Count != 0)
                {
                    foreach (var message in messages)
                    {
                        try
                        {
                            await kafkaProducer.ProduceAsync(
                                message.Topic,
                                message.Content,
                                stoppingToken);

                            logger.LogInformation(
                                "Processing outbox message {MessageId}",
                                message.Id);

                            message.MarkAsProcessed();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(
                                ex,
                                "Failed processing outbox message {MessageId}",
                                message.Id);

                            message.MarkAsError(ex.Message);
                        }
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while processing outbox messages");
            }
        }
    }
}
