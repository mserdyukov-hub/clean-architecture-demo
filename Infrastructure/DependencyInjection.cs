using System.Text;
using Application.Common.Interfaces;
using Application.Common.Messaging;
using Domain.Repositories;
using Infrastructure.Authentications;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.Messaging.Kafka;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

/// <summary>
/// Регистрация зависимостей Infrastructure слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавляет сервисы инфраструктуры: JWT, BCrypt, DbContext, UnitOfWork, репозитории
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Jwt
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // PasswordHasher
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        // DbContext
        services.AddDbContext<CaDemoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICaDemoDbContext>(
            provider => provider.GetRequiredService<CaDemoDbContext>());

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        // JWT Authentication
        var jwtSettings = new JwtSettings();
        configuration.GetSection("jwt").Bind(jwtSettings);

        services
            .AddAuthentication(options =>
            {
                // Схема по умолчанию для аутентификации (проверка токена)
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // Схема по умолчанию для вызова аутентификации (401 Unauthorized → перенаправление на логин)
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Проверять издателя токена
                    ValidateIssuer = true,

                    // Допустимый издатель
                    ValidIssuer = jwtSettings.Issuer,

                    // Проверять получателя токена
                    ValidateAudience = true,

                    // Допустимый получатель
                    ValidAudience = jwtSettings.Audience,

                    // Проверять срок действия токена
                    ValidateLifetime = true,

                    // Проверять ключ подписи
                    ValidateIssuerSigningKey = true,

                    // Ключ для проверки подписи
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings_SecretKey"] ??
                                               throw new InvalidOperationException("JWT Secret is not configured")))
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy =>
                policy.RequireRole("Admin"))
            .AddPolicy("RequireUserCreate", policy =>
                policy.RequireClaim("permission", "users.create"))
            .AddPolicy("RequireUserUpdate", policy =>
                policy.RequireClaim("permission", "users.update"))
            .AddPolicy("RequireUserDelete", policy =>
                policy.RequireClaim("permission", "users.delete"));

        services.Configure<RedisOptions>(
            configuration.GetSection(
                RedisOptions.SectionName));

        var redisOptions =
            configuration
                .GetSection(RedisOptions.SectionName)
                .Get<RedisOptions>()
            ?? throw new InvalidOperationException(
                "Redis configuration not found.");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration =
                redisOptions.ConnectionString;
        });

        services.AddScoped<ICacheService, RedisCacheService>();


        // Kafka
        services.Configure<KafkaOptions>(
            configuration.GetSection(
                KafkaOptions.SectionName));

        services.AddSingleton<IKafkaProducer,
            KafkaProducer>();

        services.AddHostedService<KafkaConsumerService>();
        // services.AddHostedService<KafkaConsumerBService>();

        return services;
    }
}
