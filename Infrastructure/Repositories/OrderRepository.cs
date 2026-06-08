using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository(CaDemoDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public void Add(Order order)
        => context.Orders.Add(order);

    public void Update(Order order)
        => context.Orders.Update(order);

    public void Delete(Order order)
        => context.Orders.Remove(order);
}
