using Contract.Common;

namespace Contract.Users;

/// <summary>
/// Событие, которое публикуется наружу при создании пользователя.
/// Это контракт для других сервисов (Email, Bonus).
/// </summary>
public sealed class UserCreatedIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// ID пользователя в системе источнике.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; init; } = null!;
}
