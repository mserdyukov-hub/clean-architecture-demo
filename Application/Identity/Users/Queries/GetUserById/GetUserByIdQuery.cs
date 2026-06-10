using Application.Common.Caching;
using MediatR;

namespace Application.Identity.Users.Queries.GetUserById;

/// <summary>
/// Запрос для получения данных пользователя и его ролях
/// </summary>
/// <param name="Id"></param>
public record GetUserByIdQuery(Guid Id) : IRequest<UserDetailDto>, ICacheable
{
    public string CacheKey => $"user:{Id}";
    public TimeSpan Expiration => TimeSpan.FromMinutes(5);
}
