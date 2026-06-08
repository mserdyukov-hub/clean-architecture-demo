using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(CaDemoDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsSplitQuery()
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        => await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsSplitQuery()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => context.Users.ToListAsync(cancellationToken);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
        => await context.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => await context.Users.AnyAsync(u => u.UserName == username, cancellationToken);

    public void Add(User user) => context.Users.Add(user);

    public void Update(User user) => context.Users.Update(user);

    public void Remove(User user) => context.Users.Remove(user);
}
