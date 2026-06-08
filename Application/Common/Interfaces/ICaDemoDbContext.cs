using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface ICaDemoDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Order> Orders { get; }

    DbSet<OrderItem> OrderItems { get; }

    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Permission> Permissions { get; }

    DbSet<RolePermission> RolePermissions { get; }
    DbSet<UserRole> UserRoles { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}
