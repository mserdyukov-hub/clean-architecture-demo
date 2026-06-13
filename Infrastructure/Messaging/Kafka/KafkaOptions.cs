namespace Infrastructure.Messaging.Kafka;

public class KafkaOptions
{
    public const string SectionName = "Kafka";
    public string BootstrapServers { get; init; } = string.Empty;

    // Имя Consumer Group
    public string GroupId { get; init; } = string.Empty;
}
