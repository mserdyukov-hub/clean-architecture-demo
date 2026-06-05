using Application.Common.Interfaces;
using Domain.ValueObjects;

namespace Infrastructure.Security;

public class BCryptPasswordHasher : IPasswordHasher
{
    public PasswordHash Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return new PasswordHash(hash);
    }

    public bool Verify(string password, PasswordHash passwordHash)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        if (passwordHash == null) // todo подумать, стоит ли делать эту проверку, если параметры не могут быть null: (ValueObject left, ValueObject right)
            throw new ArgumentNullException(nameof(passwordHash));

        return BCrypt.Net.BCrypt.Verify(password, passwordHash.Value);
    }
}