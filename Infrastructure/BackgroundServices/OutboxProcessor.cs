using Infrastructure.Data;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices;

public sealed class OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
    : BackgroundService
{
    private const int MaxRetryCount = 5;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                using var scope = scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<CaDemoDbContext>();

                var messages = await dbContext.Set<OutboxMessage>()
                    .Where(x => x.ProcessedOnUtc == null && x.RetryCount < MaxRetryCount)
                    .OrderBy(x => x.ProcessedOnUtc)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                if (messages.Count == 0)
                    continue;

                foreach (var message in messages)
                {
                    try
                    {
                        // await kafkaProducer.ProduceAsync(
                        //     topic,
                        //     message.Content,
                        //     stoppingToken);

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
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while processing outbox messages");
            }
        }
    }
}
