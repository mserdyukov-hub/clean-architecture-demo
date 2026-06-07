namespace Infrastructure.Cache;

public sealed class RedisOptions
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; init; } = string.Empty;
}
