namespace Application.Features.Shop.Categories.Queries.GetCategoryById;

public class CategoryByIdDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}
