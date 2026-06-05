using Domain.Exceptions;

namespace Domain.Entities;

public class RolePermission
{
    private RolePermission() { } // Для EF Core

    private RolePermission(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        AssignedAt = DateTime.UtcNow;
    }

    public Guid RoleId { get; }
    public Guid PermissionId { get; }
    public DateTime AssignedAt { get; }
    public Role Role { get; private set; }
    public Permission Permission { get; private set; }

    public static RolePermission Create(Role role, Permission permission)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");
        if (permission == null)
            throw new DomainException("Permission cannot be null");

        return new RolePermission(role.Id, permission.Id);
    }
}
