using Contract.Common;

namespace Application.Common.Messaging;

public interface IKafkaProducer
{
    Task ProduceAsync(string topic, IntegrationEventEnvelope envelope, CancellationToken cancellationToken = default);
}
