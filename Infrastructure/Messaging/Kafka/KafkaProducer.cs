using System.Text.Json;
using Application.Common.Messaging;
using Confluent.Kafka;
using Contract.Common;
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

    public async Task ProduceAsync(string topic, IntegrationEventEnvelope envelope,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(envelope);
        await _producer.ProduceAsync(
            topic,
            new Message<string, string> { Key = envelope.EventId.ToString(), Value = json },
            cancellationToken
        );
    }
}
