using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Категория - Корень агрегата
/// </summary>
public sealed class Category : AggregateRoot<Guid>
{
    private Category()
    {
    }

    private Category(Guid id, string name, string? description = null)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public static Category Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty.");

        return new Category(Guid.NewGuid(), name, description);
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty.");

        Name = name;
        Description = description;
    }
}
