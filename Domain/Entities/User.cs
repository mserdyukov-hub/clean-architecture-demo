using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

// Корень агрегата - сущность, через которую происходит весь доступ к связанным данным
public class User : IAggregateRoot
{
    private readonly List<UserRole> _userRoles = [];
    private const int MaxFailedAttempts = 5;
    private const int LockoutDurationMinutes = 15;

    private User(Guid id, string username, Email email, PasswordHash passwordHash)
    {
        Id = id;
        UserName = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }
    
    public Guid Id { get; }
    public string UserName { get; private set; }
    public Email Email { get; private set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public PasswordHash PasswordHash { get; }
    public DateTime CreatedAt { get; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LockoutEnd { get; set; }

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    

    public static User Create(string username, Email email, PasswordHash passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty");
        if (username.Length < 3)
            throw new DomainException("Username must be at least 3 characters");
        return new User(Guid.NewGuid(), username, email, passwordHash);
    }

    public void AssignRole(UserRole role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");
        if (_userRoles.Any(ur => ur.RoleId == role.RoleId))
            throw new DomainException("Role already assigned");
        _userRoles.Add(role);
    }

    public void RemoveRole(UserRole role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");
        
        if (_userRoles.Any(ur => ur.RoleId == role.RoleId))
            throw new DomainException($"User does not have role '{role.Role.Name}'");
        _userRoles.Remove(role);
    }

    public bool HasRole(string userRole)
        => _userRoles.Any(ur => ur.Role.Name == userRole);

    
    public void RecordSuccessfulLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        FailedLoginAttempts = 0;
        LockoutEnd = null;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;

        if (FailedLoginAttempts >= MaxFailedAttempts)
            LockoutEnd = DateTime.Now.AddMinutes(LockoutDurationMinutes); 
    }

    public void EnsureLogin()
    {
        if (!IsActive)
            throw new DomainException("User is not active");
        
        if (LockoutEnd.HasValue && LockoutEnd > DateTime.Now)
            throw new DomainException("Lockout end is earlier than the current time");
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void UpdateProfile(string username, string? firstName, string? lastName)
    {
        if(string.IsNullOrEmpty(username))
            throw new DomainException("Username cannot be empty");
        UserName = username;
        FirstName = firstName;
        LastName = lastName;
    }
}