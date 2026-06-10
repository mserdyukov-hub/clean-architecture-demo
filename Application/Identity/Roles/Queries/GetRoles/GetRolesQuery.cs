using Application.Common.Caching;
using MediatR;

namespace Application.Identity.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<List<RoleListDto>>, ICacheable
{
    public string CacheKey => "roles";
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
