namespace Application.Common.Exceptions;

/// <summary>
/// Исключение "Конфликт данных" (например, дубликат Email или Username)
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string resourceName, string field, object value)
        : base($"Resource '{resourceName}' with {field} '{value}' already exists.")
    {
    }
}