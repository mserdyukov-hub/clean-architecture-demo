using MediatR;

namespace Application.Identity.Auth.Commands.Login;

/// <summary>
/// Команда для аутентификации пользователя
/// </summary>
public record LoginCommand : IRequest<AuthResponseDto>
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
