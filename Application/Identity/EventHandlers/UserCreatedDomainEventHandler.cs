using Domain.Aggregates.Identity;
using MediatR;

namespace Application.Identity.EventHandlers;

/// <summary>
/// Обработчик события создания пользователя
/// Выполняется после сохранения пользователя в БД
/// </summary>
public sealed class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    /// <summary>
    /// Вызывается автоматически MediatR после публикации события
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"USER CREATED: {notification.Email}");

        await Task.CompletedTask;
    }
}
