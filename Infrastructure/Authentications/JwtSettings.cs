namespace Infrastructure.Authentications;

/// <summary>
/// Настройка Jwt токена
/// Значения загружаются из appsettings.json
/// Secret передается из переменных окружения
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Секретный ключ для подписи токена.
    /// </summary>
    public string Secret { get; set; } =  string.Empty;

    /// <summary>
    /// Издатель токена
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Получатель токена (для кого предназначен)
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Время жизни токена в минутах
    /// </summary>
    public int ExpirationInMinutes { get; set; } = 60;
}
