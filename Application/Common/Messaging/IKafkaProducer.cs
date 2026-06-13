namespace Application.Common.Messaging;

public interface IKafkaProducer
{
    Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default);
}
