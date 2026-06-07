using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task SeedDatabaseAsync(
        this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        using var scope = app.Services.CreateScope();

        var context =
            scope.ServiceProvider
                .GetRequiredService<IdentityDbContext>();

        var passwordHasher =
            scope.ServiceProvider
                .GetRequiredService<IPasswordHasher>();

        await DataSeeder.SeedAsync(
            context,
            passwordHasher);
    }
}
