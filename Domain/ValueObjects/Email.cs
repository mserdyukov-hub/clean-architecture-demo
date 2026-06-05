using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public class Email : ValueObject
{
    public Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");

        if (!email.Contains('@'))
            throw new DomainException("Invalid email format");

        return new Email(email.Trim().ToLowerInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
    
    public static implicit operator string(Email email) => email.Value;
}