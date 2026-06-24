using Contract.Common;
using Domain.Common;

namespace Application.Common.Messaging;

/// <summary>
/// Преобразует доменное событие в интеграционное.
/// Это граница между внутренним миром и внешним контрактом.
/// </summary>
public interface IIntegrationEventMapper
{
    // todo позже заменить на generic pipeline
    IIntegrationEvent? Map(IDomainEvent domainEvent);
}
