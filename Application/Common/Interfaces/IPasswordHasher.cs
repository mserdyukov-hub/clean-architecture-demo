using Domain.ValueObjects;

namespace Application.Common.Interfaces;

public interface IPasswordHasher
{
    /// <summary>
    /// Хеширует пароль с использованием BCrypt.
    /// Соль генерируется автоматически и сохраняется внутри хеша
    /// </summary>
    public PasswordHash Hash(string password);
    /// <summary>
    /// Проверяет соответствие пароля его хешу
    /// </summary>
    bool Verify (string password, PasswordHash passwordHash);
}