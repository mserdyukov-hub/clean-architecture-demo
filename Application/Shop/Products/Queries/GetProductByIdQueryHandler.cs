using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Messaging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Shop.Products.Queries;

public class GetProductByIdQueryHandler(ICaDemoDbContext context) : IQueryHandler<GetProductByIdQuery, ProductByIdDto>
{
    public async Task<ProductByIdDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Products.Where(p => p.Id == request.Id)
            .Select(p => new ProductByIdDto
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price.ToString(),
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
            throw new NotFoundException("Product not found", request.Id);

        return result;
    }
}
