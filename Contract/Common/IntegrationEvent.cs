namespace Contract.Common;

/// <summary>
/// Базовый контракт всех событий, которые уходят через Kafka.
/// Это НЕ доменный event. Это внешний контракт между сервисами.
/// </summary>
public abstract class IntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Уникальный идентификатор события.
    /// Используется для:
    /// - дедупликации (Inbox)
    /// - трассировки
    /// - идемпотентности
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Время, когда событие произошло в источнике.
    /// НЕ время обработки. НЕ время отправки в Kafka.
    /// </summary>
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}
