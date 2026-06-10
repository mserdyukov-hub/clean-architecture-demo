using Application.Common.Messaging;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka;

public sealed class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IOptions<KafkaOptions> options)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default)
        => await _producer.ProduceAsync(
            topic,
            new Message<string, string> { Key = message, Value = message },
            cancellationToken
            );
}
