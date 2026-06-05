using System.Reflection;
using Application.Common.Behaviors;
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

        return services;
    }
}