namespace Application.Features.Auth.Commands.Login;

/// <summary>
/// DTO для ответа с токеном после успешного логина
/// </summary>
public record AuthResponseDto
{
    /// <summary>
    /// JWT токен доступа
    /// </summary>
    public string Token { get; init; } = string.Empty;
}