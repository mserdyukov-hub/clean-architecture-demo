using Domain.Common;

namespace Infrastructure.Persistence;

public sealed class OutboxMessage : Entity<Guid>
{
    private OutboxMessage()
    {
    }

    /// <summary>
    /// Глобальный идентификатор интеграционного события.
    /// Используется для трассировки и идемпотентности.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Время, когда событие произошло в домене
    /// </summary>
    public DateTime OccurredOnUtc { get; private set; }

    /// <summary>
    /// Topic для kafka
    /// </summary>
    public string Topic { get; private set; } = null!;

    /// <summary>
    /// Тип ивента
    /// </summary>
    public string EventType { get; private set; } = null!;

    /// <summary>
    /// JSON сериализованного события
    /// </summary>
    public string Payload { get; private set; } = null!;

    /// <summary>
    /// Время успешно отработанного события ( Null - событие не отработано )
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// Последняя ошибка обработки
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Счётчик повторений
    /// </summary>
    public int RetryCount { get; private set; }

    public static OutboxMessage Create(Guid evenId, DateTime occurredOnUtc, string topic, string eventType, string payload)
        => new()
        {
            Id = Guid.NewGuid(),
            EventId = evenId,
            OccurredOnUtc = occurredOnUtc,
            Topic = topic,
            EventType = eventType,
            Payload = payload
        };

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    public void MarkAsError(string error)
    {
        RetryCount++;
        Error = error;
    }
}
