using Application.Common.Interfaces;
using Application.Common.Messaging.Mappers;
using Contract.Common;
using Domain.Common;

namespace Application.Common.Messaging;

public class IntegrationEventFactory(IServiceProvider serviceProvider) : IIntegrationEventFactory
{
    public IReadOnlyCollection<IIntegrationEvent> Create(
        IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        var integrationEvents = new List<IIntegrationEvent>();

        foreach (var domainEvent in domainEvents)
        {
            var mapperType =
                typeof(IIntegrationEventMapper<>)
                    .MakeGenericType(domainEvent.GetType());

            var mapper =
                serviceProvider.GetService(mapperType);

            if (mapper is null)
                continue;

            var mapMethod =
                mapperType.GetMethod(nameof(
                    IIntegrationEventMapper<IDomainEvent>.Map));

            if (mapMethod is null)
                continue;

            var integrationEvent =
                mapMethod.Invoke(
                    mapper,
                    [domainEvent]);

            if (integrationEvent is IIntegrationEvent @event)
            {
                integrationEvents.Add(@event);
            }
        }

        return integrationEvents;
    }
}
