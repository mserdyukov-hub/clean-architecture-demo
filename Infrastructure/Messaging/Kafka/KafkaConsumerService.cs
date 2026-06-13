using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka;

public class KafkaConsumerService(
    IOptions<KafkaOptions> options,
    ILogger<KafkaConsumerService> logger)
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
                .SetPartitionsAssignedHandler(
                    (_, partitions) =>
                    {
                        logger.LogInformation(
                            "Consumer A assigned: {Partitions} at {time}",
                            string.Join(", ", partitions), DateTime.Now);
                    })
                .Build();
        // подписываемся на topic
        consumer.Subscribe("users-topic");

        logger.LogInformation("Kafka Consumer started");

        try
        {
            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(cancellationToken);

                    logger.LogInformation(
                        """
                        Message received

                        Topic: {Topic}
                        Partition: {Partition}
                        Offset: {Offset}
                        Key: {Key}
                        Value: {Value}
                        """,
                        result.Topic,
                        result.Partition.Value,
                        result.Offset.Value,
                        result.Message.Key,
                        result.Message.Value);

                    //
                    // Имитируем бизнес-логику
                    //
                    ProcessMessage(result);

                    //
                    // Commit только после успешной обработки
                    //
                    consumer.Commit(result);


                    logger.LogInformation(
                        """
                        Offset committed

                        Partition: {Partition}
                        Next offset: {NextOffset}
                        """,
                        result.Partition.Value,
                        result.Offset.Value + 1);

                }
            }, cancellationToken);

        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Kafka Consumer stopped");
        }
        catch (ConsumeException e)
        {
            logger.LogCritical(e, "Kafka Consumer error");
        }
        finally
        {
            consumer.Close();
        }
    }

    private static void ProcessMessage(
        ConsumeResult<string, string> result)
    {
        Thread.Sleep(1000);
    }
}
