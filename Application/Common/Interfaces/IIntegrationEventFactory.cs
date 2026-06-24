using Contract.Common;
using Domain.Common;

namespace Application.Common.Interfaces;

public interface IIntegrationEventFactory
{
    IReadOnlyCollection<IIntegrationEvent> Create(IReadOnlyCollection<IDomainEvent> domainEvents);
}
