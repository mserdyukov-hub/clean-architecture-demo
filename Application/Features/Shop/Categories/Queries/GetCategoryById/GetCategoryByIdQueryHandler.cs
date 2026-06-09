using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Shop.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(ICaDemoDbContext context)
    : IQueryHandler<GetCategoryByIdQuery, CategoryByIdDto>
{
    public async Task<CategoryByIdDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Categories.Where(c => c.Id == request.Id)
            .Select(c => new CategoryByIdDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
            throw new NotFoundException("Category not found", request.Id);

        return result;
    }
}
