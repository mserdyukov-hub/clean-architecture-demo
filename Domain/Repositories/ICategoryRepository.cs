using Domain.Entities;

namespace Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Category?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    void Add( Category category);

    void Update(Category category);

    void Delete(Category category);
}
