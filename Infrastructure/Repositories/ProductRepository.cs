using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository(CaDemoDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Products.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => await context.Products.AnyAsync(p => p.Name == name, cancellationToken);

    public void Add(Product product)
        => context.Products.Add(product);

    public void Update(Product product)
        => context.Products.Update(product);

    public void Remove(Product product)
        => context.Products.Remove(product);
}
