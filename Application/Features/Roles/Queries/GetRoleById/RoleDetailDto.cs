namespace Application.Features.Roles.Queries.GetRoleById;

public record RoleDetailDto
{
    /// <summary>
    /// Id роли
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Наименование роли
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Описание роли
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Дата создания роли
    /// </summary>
    public DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// Системная роль
    /// </summary>
    public bool IsSystem { get; init; }
    
    /// <summary>
    /// Список доступов
    /// </summary>
    public List<string> Permissions { get; init; }
}