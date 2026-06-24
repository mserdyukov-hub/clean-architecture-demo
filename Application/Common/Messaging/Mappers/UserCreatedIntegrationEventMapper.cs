using Application.Common.Interfaces;
using Contract.Common;
using Contract.Users;
using Domain.Aggregates.Identity;

namespace Application.Common.Messaging.Mappers;

public class UserCreatedIntegrationEventMapper : IIntegrationEventMapper<UserCreatedDomainEvent>
{
    /// <summary>
    /// Преобразует доменное событие создания пользователя во внешний контракт Kafka.
    /// </summary>
    public IIntegrationEvent Map(UserCreatedDomainEvent domainEvent)
    {
        return new UserCreatedIntegrationEvent
        {
            UserId = domainEvent.Id,
            Email = domainEvent.Email,
            OccurredOnUtc = domainEvent.OccurredOnUtc
        };
    }
}
