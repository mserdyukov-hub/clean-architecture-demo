using Contract.Common;
using Domain.Common;

namespace Application.Common.Messaging.Mappers;

/// <summary>
/// Преобразует конкретное доменное событие в конкретное интеграционное событие.
/// </summary>
public interface IIntegrationEventMapper<in TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    IIntegrationEvent Map(TDomainEvent domainEvent);
}
