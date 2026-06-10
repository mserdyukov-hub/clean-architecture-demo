using Domain.Common;

namespace Infrastructure.Persistence;

public sealed class OutboxMessage : Entity<Guid>
{
    private OutboxMessage()
    {
    }

    /// <summary>
    /// Время, когда событие произошло в домене
    /// </summary>
    public DateTime OccurredOnUtc { get; private set; }

    /// <summary>
    /// Тип ивента
    /// </summary>
    public string Type { get; private set; } = null!;

    /// <summary>
    /// JSON сериализованного события
    /// </summary>
    public string Content { get; private set; } = null!;

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

    public static OutboxMessage Create(DateTime occurredOnUtc, string type, string content)
        => new() { Id = Guid.NewGuid(), OccurredOnUtc = occurredOnUtc, Type = type, Content = content };

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
