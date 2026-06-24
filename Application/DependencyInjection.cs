using System.Reflection;
using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Common.Messaging;
using Application.Common.Messaging.Mappers;
using Domain.Aggregates.Identity;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Регистрация зависимостей Application слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавляет сервисы Application слоя: MediatR, FluentValidation, ValidationBehavior
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR (автоматически находит все Handler'ы в сборке)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // FluentValidation (автоматически находит все Validator'ы в сборке)
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // ValidationBehavior (пайплайн валидации)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // LoggingBehavior (пайплайн логирования)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // CacheBehavior (пайплайн кеширования)
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

        services.AddScoped<IIntegrationEventMapper<UserCreatedDomainEvent>, UserCreatedIntegrationEventMapper>();

        services.AddScoped<IIntegrationEventFactory, IntegrationEventFactory>();

        return services;
    }
}
