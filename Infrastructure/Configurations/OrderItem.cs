using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Infrastructure.Configurations;

public sealed class OrderItem : Entity<Guid>
{
    private OrderItem()
    {
    }

    private OrderItem(
        Guid id,
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = null!;

    public Money UnitPrice { get; private set; } = null!;

    public int Quantity { get; private set; }

    public static OrderItem Create(
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        return new OrderItem(
            Guid.NewGuid(),
            productId,
            productName,
            unitPrice,
            quantity);
    }
}
