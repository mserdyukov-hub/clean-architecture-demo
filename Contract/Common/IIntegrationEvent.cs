namespace Contract.Common;

/// <summary>
/// Базовый контракт всех интеграционных событий.
/// Гарантирует наличие метаданных события.
/// </summary>
public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredOnUtc { get; }
}
