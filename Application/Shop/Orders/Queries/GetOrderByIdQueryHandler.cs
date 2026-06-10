using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Application.Shop.Orders.Queries;

public class GetOrderByIdQueryHandler(ICaDemoDbContext context) : IQueryHandler<GetOrderByIdQuery, OrderByIdDto>
{
    public async Task<OrderByIdDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Orders
            .Where(o => o.Id == request.Id)
            .Select(o => new OrderByIdDto
            {
                Status = o.Status.ToString(),
                Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice.ToString(),
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            throw new NotFoundException("Order not found", request.Id);

        return result;
    }
}
