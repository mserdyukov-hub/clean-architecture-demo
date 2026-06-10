using Application.Common.Caching;
using MediatR;

namespace Application.Identity.Permissions.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<List<PermissionListDto>>, ICacheable
{
    public string CacheKey => "permissions";
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
