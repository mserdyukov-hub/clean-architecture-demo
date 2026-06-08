using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PermissionRepository(CaDemoDbContext context) : IPermissionRepository
{
    public async Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Permissions.ToListAsync(cancellationToken);

    public async Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Permissions.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => await context.Permissions.FirstOrDefaultAsync(p => p.Code == code, cancellationToken);

    public async Task<List<Permission>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        => await context.Permissions.Where(r => r.RolePermissions.Any(rp => rp.RoleId == roleId))
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
        => await context.Permissions.AnyAsync(p => p.Code == code, cancellationToken);

    public void Add(Permission permission) => context.Permissions.Add(permission);

    public void Remove(Permission permission) => context.Permissions.Remove(permission);
}