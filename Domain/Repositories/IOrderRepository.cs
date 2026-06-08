using Domain.Entities;

namespace Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    void Add(Order order);

    void Update(Order order);

    void Delete(Order order);
}
