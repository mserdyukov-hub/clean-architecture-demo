using MediatR;

namespace Domain.Common;

/// <summary>
/// Базовый интерфейс для всех доменных событий
/// Наследуется от MediatR INotification, чтобы события можно было публиковать через MediatR
/// </summary>
public interface IDomainEvent : INotification
{
    DateTime OccurredOnUtc { get; }
}
