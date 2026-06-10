using Application.Common.Caching;
using Application.Common.Messaging;

namespace Application.Shop.Orders.Queries;

public record GetOrderByIdQuery(Guid Id) : IQuery<OrderByIdDto>, ICacheable
{
    public string CacheKey { get; } = $"order:{Id}";
    public TimeSpan Expiration { get; } = TimeSpan.FromMinutes(5);
}
