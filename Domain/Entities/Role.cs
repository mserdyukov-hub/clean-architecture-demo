using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

// Корень агрегата - сущность, через которую происходит весь доступ к связанным данным
public class Role : IAggregateRoot
{
    private readonly List<UserRole> _userRoles = [];
    private readonly List<RolePermission> _rolePermissions = [];

    private Role()
    {
    }

    public Role(string name, string? description = null, bool isSystem = false)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        IsSystem = isSystem;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; }
    public bool IsSystem { get; }

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    public static Role Create(string name, string? description) => new(name, description);

    public static Role CreateSystemRole(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("System role description cannot be empty");

        return new Role(name, description, true);
    }

    public void AddPermission(Permission permission)
    {
        if (permission == null)
            throw new DomainException("Permission cannot be null");

        if (_rolePermissions.Any(rp => rp.PermissionId == permission.Id))
            throw new DomainException($"Role already has permission '{permission.Name}'");

        _rolePermissions.Add(RolePermission.Create(this, permission));
    }

    public void RemovePermission(Permission permission)
    {
        if (permission == null)
            throw new DomainException("Permission cannot be null");

        var rolePermission = _rolePermissions.FirstOrDefault(rp => rp.PermissionId == permission.Id);

        if (rolePermission == null)
            throw new DomainException($"Role does not have permission '{permission.Name}'");

        _rolePermissions.Remove(rolePermission);
    }

    public bool HasPermission(Permission permission)
        => _rolePermissions.Any(rp => rp.PermissionId == permission.Id);

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty");

        if (IsSystem)
            throw new DomainException("System role cannot be updated");
        
        if (name.Length is < 2 or > 50)
            throw new DomainException("Name must be between 2 and 50 characters");

        Name = name;
    }
}