using Application.Common.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Persistence;

public class UnitOfWork(IdentityDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
