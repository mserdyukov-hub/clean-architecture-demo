using Domain.Common;

namespace Infrastructure.Persistence;

public sealed class InboxMessage : Entity<Guid>
{
    /// <summary>
    /// Kafka topic
    /// </summary>
    public string Topic { get; private set; } = null!;

    /// <summary>
    /// Kafka partition
    /// </summary>
    public int Partition { get; private set; }

    /// <summary>
    /// Kafka offset (уникальный идентификатор сообщения в рамках partition)
    /// </summary>
    public long Offset { get; private set; }

    /// <summary>
    /// Время получения сообщения
    /// </summary>
    public DateTime ReceivedOnUtc { get; private set; }

    /// <summary>
    /// Время успешной обработки
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// Ошибка последней обработки
    /// </summary>
    public string? Error { get; private set; }

    private InboxMessage()
    {
    }

    public static InboxMessage Create(
        string topic,
        int partition,
        long offset)
    {
        return new InboxMessage
        {
            Id = Guid.NewGuid(),
            Topic = topic,
            Partition = partition,
            Offset = offset,
            ReceivedOnUtc = DateTime.UtcNow
        };
    }

    public void MarkProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    public void MarkFailed(string message)
    {
        Error = message;
    }
}
