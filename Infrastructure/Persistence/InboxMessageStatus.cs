namespace Infrastructure.Persistence;

/// <summary>
/// Состояние обработки входящего интеграционного события.
/// </summary>
public enum InboxMessageStatus
{
    Processing = 1,

    Completed = 2,

    Failed = 3
}
