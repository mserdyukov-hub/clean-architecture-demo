namespace Application.Common.Exceptions;

/// <summary>
/// Исключение "Ресурс не найден"
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
    
    public NotFoundException(string resourceName, object resourceKey)
        : base($"Resource '{resourceName}' with key '{resourceKey}' was not found.")
    {
    }
}