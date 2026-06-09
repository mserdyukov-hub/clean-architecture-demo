using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(
        CaDemoDbContext context,
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
        var adminRole = Role.CreateSystemRole(
            SystemRoleIds.Admin,
            "Admin",
            "System administrator");

        var managerRole = Role.CreateSystemRole(
            SystemRoleIds.Manager,
            "Manager",
            "Store manager");

        var sellerRole = Role.CreateSystemRole(
            Guid.NewGuid(),
            "Seller",
            "Store seller");

        var userRole = Role.CreateSystemRole(
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

        sellerRole.AddPermission(usersRead);

        userRole.AddPermission(usersRead);

        context.Roles.AddRange(
            adminRole,
            managerRole,
            sellerRole,
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

        var seller = User.Create(
            "seller",
            new Email("seller@test.com"),
            passwordHasher.Hash("Seller123!"));

        seller.UpdateProfile(
            "seller",
            "Store",
            "Seller");

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

        seller.AssignRole(
            UserRole.Create(
                seller,
                sellerRole));

        user.AssignRole(
            UserRole.Create(
                user,
                userRole));

        context.Users.AddRange(
            admin,
            manager,
            seller,
            user);

        //
        // Categories
        //
        var electronics = Category.Create(
            "Electronics",
            "Electronic devices and gadgets");

        var books = Category.Create(
            "Books",
            "Books and literature");

        var home = Category.Create(
            "Home",
            "Home and kitchen products");

        context.Categories.AddRange(
            electronics,
            books,
            home);

        //
        // Products
        //
        var laptop = Product.Create(
            "Dell XPS 15",
            "Powerful business laptop",
            Money.Create(1800m),
            10,
            electronics.Id);

        var keyboard = Product.Create(
            "Mechanical Keyboard",
            "RGB mechanical keyboard",
            Money.Create(120m),
            30,
            electronics.Id);

        var monitor = Product.Create(
            "LG UltraWide",
            "34 inch ultrawide monitor",
            Money.Create(650m),
            15,
            electronics.Id);

        var cleanArchitecture = Product.Create(
            "Clean Architecture",
            "Robert C. Martin",
            Money.Create(45m),
            100,
            books.Id);

        var coffeeMachine = Product.Create(
            "Coffee Machine",
            "Automatic coffee machine",
            Money.Create(350m),
            8,
            home.Id);

        context.Products.AddRange(
            laptop,
            keyboard,
            monitor,
            cleanArchitecture,
            coffeeMachine);

        //
        // Orders
        //
        var order1 = Order.Create(user.Id);

        order1.AddItem(
            laptop.Id,
            laptop.Name,
            laptop.Price,
            1);

        order1.AddItem(
            keyboard.Id,
            keyboard.Name,
            keyboard.Price,
            2);

        order1.Confirm();

        var order2 = Order.Create(manager.Id);

        order2.AddItem(
            cleanArchitecture.Id,
            cleanArchitecture.Name,
            cleanArchitecture.Price,
            3);

        order2.Confirm();
        order2.Complete();

        var order3 = Order.Create(user.Id);

        order3.AddItem(
            coffeeMachine.Id,
            coffeeMachine.Name,
            coffeeMachine.Price,
            1);

        order3.Cancel();

        context.Orders.AddRange(
            order1,
            order2,
            order3);

        await context.SaveChangesAsync(cancellationToken);
    }
}
