using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

// Корень агрегата - сущность, через которую происходит весь доступ к связанным данным
public class Permission : IAggregateRoot
{
    private readonly List<RolePermission> _rolePermissions = [];

    private Permission()
    {
    }

    private Permission(Guid id, string name, string code, string group, string? description = null)
    {
        Id = id;
        Name = name;
        Code = code;
        Group = group;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Code { get; }
    public string Group { get; }
    public DateTime CreatedAt { get; }

    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    public static Permission Create(Guid id, string name, string code, string group, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Permission code cannot be empty.");
        if (string.IsNullOrWhiteSpace(group))
            throw new DomainException("Permission group cannot be empty.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Permission name cannot be empty.");

        return new Permission(id, name, code, group, description);
    }

    public void UpdateInfo(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Permission name cannot be empty.");

        Name = name;
        Description = description;
    }

    public void AddRole(Role role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null.");

        if (_rolePermissions.Any(rp => rp.RoleId == role.Id))
            throw new DomainException("Role already exists.");

        _rolePermissions.Add(RolePermission.Create(role, this));
    }

    public void RemoveRole(Role role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null.");

        var rolePermission = _rolePermissions.FirstOrDefault(rp => rp.RoleId == role.Id);

        if (rolePermission == null)
            throw new DomainException("Role does not exist.");

        _rolePermissions.Remove(rolePermission);
    }
}
