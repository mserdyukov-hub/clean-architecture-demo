using Domain.Aggregates.Identity;
using MediatR;

namespace Application.Identity.EventHandlers;

public class UserCreatedBonusHandler : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"BONUS: Granted for {notification.Email}");

        await Task.CompletedTask;
    }
}
