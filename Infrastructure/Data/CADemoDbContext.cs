using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class CaDemoDbContext(DbContextOptions<CaDemoDbContext> options, IMediator mediator)
    : DbContext(options), ICaDemoDbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<OutboxMessage>  OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CaDemoDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

       // await PublishDomainEventsAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// Находит все доменные события у агрегатов и публикует их через MediatR
    /// </summary>
    // private async Task PublishDomainEventsAsync(
    //     CancellationToken cancellationToken)
    // {
    //     // Получаем все агрегаты, у которых есть хотя бы одно доменное событие
    //     var aggregates = ChangeTracker
    //         .Entries<AggregateRoot<Guid>>()
    //         .Select(x => x.Entity)
    //         .Where(x => x.DomainEvents.Count != 0)
    //         .ToList();
    //
    //     // Собираем все события из всех агрегатов в один список
    //     var domainEvents = aggregates
    //         .SelectMany(x => x.DomainEvents)
    //         .ToList();
    //
    //     // Публикуем каждое событие через MediatR
    //     foreach (var domainEvent in domainEvents)
    //     {
    //         await mediator.Publish(
    //             domainEvent,
    //             cancellationToken);
    //     }
    //
    //     // Очищаем события внутри агрегатов
    //     foreach (var aggregate in aggregates)
    //     {
    //         aggregate.ClearDomainEvents();
    //     }
    // }
}
