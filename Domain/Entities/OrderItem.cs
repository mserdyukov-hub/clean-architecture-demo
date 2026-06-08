using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class OrderItem : Entity<Guid>
{
    private OrderItem()
    {
    }

    private OrderItem(
        Guid id,
        Guid orderId,
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

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = null!;

    public Money UnitPrice { get; private set; } = null!;

    public int Quantity { get; private set; }

    public static OrderItem Create(
        Guid orderId,
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId cannot be empty.");

        if (productId == Guid.Empty)
            throw new DomainException("ProductId cannot be empty.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        return new OrderItem(
            Guid.NewGuid(),
            orderId,
            productId,
            productName,
            unitPrice,
            quantity);
    }
}
