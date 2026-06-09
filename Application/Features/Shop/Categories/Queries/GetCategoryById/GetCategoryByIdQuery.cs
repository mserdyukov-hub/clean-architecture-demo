using Application.Common.Caching;
using Application.Common.Messaging;

namespace Application.Features.Shop.Categories.Queries.GetCategoryById;

/// <summary>
/// Запрос для получения категорий товаров
/// </summary>
/// <param name="Id"></param>
public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryByIdDto>, ICacheable
{
    public string CacheKey { get; } = $"category:{Id}";
    public TimeSpan Expiration { get; } = TimeSpan.FromMinutes(5);
}
