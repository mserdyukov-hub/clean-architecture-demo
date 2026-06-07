namespace Domain.Constants;

public static class PermissionIds
{
    public static readonly Guid UsersRead =
        Guid.Parse("10000000-0000-0000-0000-000000000001");

    public static readonly Guid UsersCreate =
        Guid.Parse("10000000-0000-0000-0000-000000000002");

    public static readonly Guid UsersUpdate =
        Guid.Parse("10000000-0000-0000-0000-000000000003");

    public static readonly Guid UsersDelete =
        Guid.Parse("10000000-0000-0000-0000-000000000004");
}
