namespace Application.Features.Shop.Orders.Queries;

public class OrderByIdDto
{
    public string Status { get; set; } = null!;

    public List<OrderItemDto> Items { get; set; } = null!;
}

public class OrderItemDto
{
    public string ProductName { get; set; } = null!;

    public string UnitPrice { get;  set; } = null!;

    public int Quantity { get;  set; }
}
