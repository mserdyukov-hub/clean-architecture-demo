using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository(CaDemoDbContext context) : IRoleRepository
{
    public async Task<List<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Roles.ToListAsync(cancellationToken);

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<Role?> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);

    public async Task<List<Role>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await context.Roles
            .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => await context.Roles.AnyAsync(r => r.Name == name, cancellationToken);

    public void Add(Role role) => context.Roles.Add(role);

    public void Remove(Role role) => context.Roles.Remove(role);
}