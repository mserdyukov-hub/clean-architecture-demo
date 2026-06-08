using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Infrastructure.Configurations;

public sealed class Order : Entity<Guid>, IAggregateRoot
{

    private readonly List<OrderItem> _items = [];

    private Order()
    {
    }

    private Order(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
    }

    public Guid UserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public static Order Create(Guid userId)
    {
        return new Order(
            Guid.NewGuid(),
            userId);
    }

    public void AddItem(
        Guid productId,
        string productName,
        Money unitPrice,
        int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        _items.Add(
            OrderItem.Create(
                productId,
                productName,
                unitPrice,
                quantity));
    }

    public Money TotalAmount =>
        Money.Create(
            _items.Sum(x => x.UnitPrice.Amount * x.Quantity));

    public void Confirm()
    {
        if (_items.Count == 0)
            throw new DomainException("Order must contain at least one item.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new DomainException("Completed order cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException("Only confirmed orders can be completed.");

        Status = OrderStatus.Completed;
    }
}
