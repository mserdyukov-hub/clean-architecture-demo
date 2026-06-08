using Application.Common.Caching;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<List<UserListDto>> , ICacheable
{
    public string CacheKey => "users";
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
