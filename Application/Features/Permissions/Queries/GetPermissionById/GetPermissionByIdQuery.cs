using Application.Common.Caching;
using MediatR;

namespace Application.Features.Permissions.Queries.GetPermissionById;

public record GetPermissionByIdQuery(Guid Id) : IRequest<PermissionDetailDto>, ICacheable
{
    public string CacheKey => $"permission:{Id}";
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
