using Application.Common.Messaging;

namespace Application.Identity.Users.Commands.CreateUser;

/// <summary>
/// Команда для создания нового пользователя
/// </summary>
public class CreateUserCommand : ICommand<Guid>
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; set; }
}
