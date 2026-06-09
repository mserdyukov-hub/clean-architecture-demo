using Domain.Aggregates.Identity;
using MediatR;

namespace Application.Identity.EventHandlers;

public class UserCreatedAuditHandler : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"AUDIT: User {notification.Id} created");

        await Task.CompletedTask;
    }
}
