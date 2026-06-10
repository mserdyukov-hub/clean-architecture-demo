using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Продукт - Корень агрегата
/// </summary>
public sealed class Product : AggregateRoot<Guid>
{
    private Product()
    {
    }

    private Product(Guid id, string name, string description, Money price, int stockQuantity, Guid categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        CategoryId = categoryId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public Money Price { get; private set; } = null!;

    public int StockQuantity { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Guid CategoryId { get; private set; }


    public static Product Create(string name, string description, Money price, int stockQuantity,
        Guid categoryId)
    {
        ValidateDetails(name, description, categoryId);

        if (price is null)
            throw new DomainException("Product price cannot be null.");

        if (stockQuantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");

        return new Product(Guid.NewGuid(), name, description, price, stockQuantity, categoryId);
    }

    public void Update(string name, string description, Money price, int stockQuantity, Guid categoryId)
    {
        ValidateDetails(name, description, categoryId);

        Name = name.Trim();
        Description = description.Trim();
        CategoryId = categoryId;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void UpdatePrice(Money price)
        => Price = price;

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (StockQuantity < quantity)
            throw new DomainException("Stock quantity cannot be negative.");

        StockQuantity -= quantity;
    }

    public void Activate()
        => IsActive = true;

    public void Deactivate()
        => IsActive = false;

    private static void ValidateDetails(string name, string description, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product description cannot be null or whitespace.");

        if (categoryId == Guid.Empty)
            throw new DomainException("CategoryId cannot be empty.");
    }
}
