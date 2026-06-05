namespace Application.Common.Exceptions;

/// <summary>
/// Исключение "Доступ запрещён" (недостаточно прав)
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string action, string reason)
        : base($"Access denied for action '{action}'. Reason: {reason}.")
    {
    }
}