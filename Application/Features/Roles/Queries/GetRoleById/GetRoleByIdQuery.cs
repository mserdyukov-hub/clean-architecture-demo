using Application.Common.Caching;
using MediatR;

namespace Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDetailDto>, ICacheable
{
    public string CacheKey => $"role:{Id}";
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
