using Application.Common.Caching;
using Application.Common.Messaging;

namespace Application.Shop.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductByIdDto>, ICacheable
{
    public string CacheKey { get; } = $"product:{Id}";
    public TimeSpan Expiration { get; } = TimeSpan.FromMinutes(5);
}
