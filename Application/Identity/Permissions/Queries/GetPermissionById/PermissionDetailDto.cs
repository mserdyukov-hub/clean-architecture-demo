namespace Application.Identity.Permissions.Queries.GetPermissionById;

public class PermissionDetailDto
{
    /// <summary>
    /// Id разрешения
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование разрешения
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание разрешения
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Код разрешения
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Группа разрешения
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// Дата создания разрешения
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
