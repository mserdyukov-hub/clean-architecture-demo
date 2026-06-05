using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

// Domain не должен знать о криптографии: HashPassword(string password), VerifyPassword(string password, string hash).
// Создан IPasswordHasher в Application слое. Реализация BCryptPasswordHasher в Interface слое.
public class PasswordHash : ValueObject
{
    public string Value { get; }

    public PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Password hash value cannot be null or whitespace.");
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}