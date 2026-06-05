using Domain.Exceptions;

namespace Domain.Entities;

public class UserRole
{
    private UserRole()
    {
    }

    private UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedAt = DateTime.Now;
    }

    public Guid UserId { get; }
    public Guid RoleId { get; }
    public DateTime AssignedAt { get; }
    
    public User User { get; private set; }
    public Role Role { get; private set; }

    public static UserRole Create(User user, Role role)
    {
        if (user == null)
            throw new DomainException("User is null");
        if (role == null)
            throw new DomainException("Role is null");
        
        return new UserRole(user.Id, role.Id);
    }
}