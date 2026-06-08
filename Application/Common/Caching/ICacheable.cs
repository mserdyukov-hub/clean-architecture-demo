namespace Application.Common.Caching;

public interface ICacheable
{
    string CacheKey { get; }
    TimeSpan Expiration { get; }
}
