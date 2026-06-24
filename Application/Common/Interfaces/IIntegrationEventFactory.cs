using Contract.Common;
using Domain.Common;

namespace Application.Common.Interfaces;

/// <summary>
/// Это единственная точка преобразования Domain → Integration
/// </summary>
public interface IIntegrationEventFactory
{
    IReadOnlyCollection<IIntegrationEvent> Create(IReadOnlyCollection<IDomainEvent> domainEvents);
}
