using Domain.Entities;

namespace Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    void Add(Product product);

    void Update(Product product);

    void Remove(Product product);
}
