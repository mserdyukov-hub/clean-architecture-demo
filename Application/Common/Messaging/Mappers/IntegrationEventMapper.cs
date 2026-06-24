using Contract.Common;
using Contract.Users;
using Domain.Aggregates.Identity;
using Domain.Common;

namespace Application.Common.Messaging.Mappers;

public class IntegrationEventMapper : IIntegrationEventMapper
{
    public IIntegrationEvent? Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            UserCreatedDomainEvent e => new UserCreatedIntegrationEvent
            {
                UserId = e.Id,
                Email = e.Email,

                //фиксируем время события из домена
                OccurredOnUtc = e.OccurredOnUtc
            },
            _ => null
        };
    }
}
