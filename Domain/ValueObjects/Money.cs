using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public sealed class Money(string currency, decimal amount) : ValueObject
{
    public decimal Amount { get; } = amount;
    public string Currency { get; } = currency;

    public static Money Create(decimal amount, string currency = "EUR")
    {
        if (amount < 0)
            throw new DomainException("Money amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Money currency cannot be empty.");

        return new Money(currency, amount);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString()
        => $"{Amount:F2} {Currency}";
}
