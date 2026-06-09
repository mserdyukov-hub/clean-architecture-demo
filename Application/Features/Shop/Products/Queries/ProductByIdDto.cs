namespace Application.Features.Shop.Products.Queries;

public class ProductByIdDto
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Price { get; set; } = null!;

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; }
}
