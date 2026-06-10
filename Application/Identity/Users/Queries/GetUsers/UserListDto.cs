namespace Application.Identity.Users.Queries.GetUsers;

public record UserListDto
{
    /// <summary>
    ///  Id пользователя
    /// </summary>
    public Guid Id { get; init; }

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
}
