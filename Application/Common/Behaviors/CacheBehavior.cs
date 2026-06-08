using Application.Common.Caching;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Common.Behaviors;

public sealed class CacheBehavior<TRequest, TResponse>(ICacheService cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheable
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cache = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

        if (cache is not null)
            return cache;

        var response = await next(cancellationToken);

        await cacheService.SetAsync(request.CacheKey, response, request.Expiration, cancellationToken);

        return response;
    }
}
