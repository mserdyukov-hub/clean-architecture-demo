namespace Contract.Common;

/// <summary>
/// Универсальная оболочка сообщения, публикуемого в Kafka
/// </summary>
public sealed class IntegrationEventEnvelope
{
    /// <summary>
    /// Уникальный идентификатор события
    /// </summary>
    public Guid EventId { get; init; }

    /// <summary>
    /// Тип события
    /// Например: UserCreatedIntegrationEvent
    /// </summary>
    public string EventType { get; init; } = null!;

    /// <summary>
    /// Время возникновения события
    /// </summary>
    public DateTime OccurredOnUtc { get; init; }

    /// <summary>
    /// Полезная нагрузка события.
    /// JSON конкретного IntegrationEvent
    /// </summary>
    public string Payload { get; init; } = null!;
}
