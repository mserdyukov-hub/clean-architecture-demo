using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(
        IdentityDbContext context,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default)
    {
        if (await context.Users.AnyAsync(cancellationToken))
            return;

        //
        // Permissions
        //
        var usersRead = Permission.Create(
            PermissionIds.UsersRead,
            "Read Users",
            "users.read",
            "Users");

        var usersCreate = Permission.Create(
            PermissionIds.UsersCreate,
            "Create Users",
            "users.create",
            "Users");

        var usersUpdate = Permission.Create(
            PermissionIds.UsersUpdate,
            "Update Users",
            "users.update",
            "Users");

        var usersDelete = Permission.Create(
            PermissionIds.UsersDelete,
            "Delete Users",
            "users.delete",
            "Users");

        context.Permissions.AddRange(
            usersRead,
            usersCreate,
            usersUpdate,
            usersDelete);

        //
        // Roles
        //
        var adminRole =
            Role.CreateSystemRole(
                SystemRoleIds.Admin,
                "Admin",
                "System administrator");

        var managerRole =
            Role.CreateSystemRole(
                SystemRoleIds.Manager,
                "Manager",
                "Store manager");

        var userRole =
            Role.CreateSystemRole(
                SystemRoleIds.User,
                "User",
                "Regular user");

        //
        // Role -> Permission
        //
        adminRole.AddPermission(usersRead);
        adminRole.AddPermission(usersCreate);
        adminRole.AddPermission(usersUpdate);
        adminRole.AddPermission(usersDelete);

        managerRole.AddPermission(usersRead);
        managerRole.AddPermission(usersUpdate);

        userRole.AddPermission(usersRead);

        context.Roles.AddRange(
            adminRole,
            managerRole,
            userRole);

        //
        // Users
        //
        var admin = User.Create(
            "admin",
            new Email("admin@test.com"),
            passwordHasher.Hash("Admin123!"));

        admin.UpdateProfile(
            "admin",
            "System",
            "Administrator");

        var manager = User.Create(
            "manager",
            new Email("manager@test.com"),
            passwordHasher.Hash("Manager123!"));

        manager.UpdateProfile(
            "manager",
            "Store",
            "Manager");

        var user = User.Create(
            "user",
            new Email("user@test.com"),
            passwordHasher.Hash("User123!"));

        user.UpdateProfile(
            "user",
            "Regular",
            "User");

        //
        // User -> Role
        //
        admin.AssignRole(
            UserRole.Create(
                admin,
                adminRole));

        manager.AssignRole(
            UserRole.Create(
                manager,
                managerRole));

        user.AssignRole(
            UserRole.Create(
                user,
                userRole));

        context.Users.AddRange(
            admin,
            manager,
            user);

        await context.SaveChangesAsync(cancellationToken);
    }
}
