namespace Application.Common.Caching;

public static class CacheKeys
{
    public static string User(Guid id)
        => $"user:{id}";

    public static string Users()
        => "users";

    public static string Role(Guid id)
        => $"role:{id}";

    public static string Roles()
        => "roles";

    public static string Permission(Guid id)
        => $"permission:{id}";

    public static string Permissions()
        => "permissions";
}
