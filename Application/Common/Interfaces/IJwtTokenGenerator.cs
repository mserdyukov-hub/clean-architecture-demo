using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Сервис генерации JWT токенов
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Генерирует JWT токен для пользователя.
    /// В токен включаются: userId, email, username, роли и разрешения пользователя
    /// </summary>
    string GenerateToken(User user);
}