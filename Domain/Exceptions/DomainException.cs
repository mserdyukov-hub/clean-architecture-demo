namespace Domain.Exceptions;

// Самописный Exception. Делает код самодокументированным. Позволяет отличать бизнес-ошибки от технических
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}