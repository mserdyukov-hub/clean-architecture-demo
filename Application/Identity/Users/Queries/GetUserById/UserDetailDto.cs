namespace Application.Identity.Users.Queries.GetUserById;

/// <summary>
/// DTO для отображения информации о пользователе
/// </summary>
public class UserDetailDto
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Активен ли пользователь
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Дата последнего входа
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Роли пользователя
    /// </summary>
    public List<string> Roles { get; set; } = [];
}
