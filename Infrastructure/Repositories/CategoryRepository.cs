using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository(CaDemoDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Categories.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await context.Categories.SingleOrDefaultAsync(c => c.Name == name, cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => await context.Categories.AnyAsync(c => c.Name == name, cancellationToken);

    public void Add(Category category)
        => context.Categories.Add(category);

    public void Update(Category category)
        => context.Categories.Update(category);

    public void Delete(Category category)
        => context.Categories.Remove(category);
}
