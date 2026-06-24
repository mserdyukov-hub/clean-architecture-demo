using Domain.Common;

namespace Infrastructure.Persistence;

public sealed class InboxMessage : Entity<Guid>
{
    /// <summary>
    /// Глобальный идентификатор интеграционного события.
    /// Приходит из IntegrationEventEnvelope.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Имя Consumer-а
    /// Например:
    /// EmailConsumer
    /// BonusConsumer
    /// </summary>
    public string ConsumerName { get; private set; } = null!;

    /// <summary>
    /// Время получения события
    /// </summary>
    public DateTime ReceivedOnUtc { get; private set; }

    /// <summary>
    /// Время успешного завершения обработки
    /// </summary>
    public DateTime? ProcessedOnUtc { get; private set; }

    /// <summary>
    /// Текущий статус обработки
    /// </summary>
    public InboxMessageStatus Status { get; private set; }

    /// <summary>
    /// Последняя ошибка обработки
    /// </summary>
    public string? Error { get; private set; }

    private InboxMessage()
    {
    }

    /// <summary>
    /// Создаёт запись о начале обработки события
    /// </summary>
    public static InboxMessage Create(
        Guid eventId,
        string consumerName)
    {
        return new InboxMessage
        {
            Id = Guid.NewGuid(),

            EventId = eventId,

            ConsumerName = consumerName,

            ReceivedOnUtc = DateTime.UtcNow,

            Status = InboxMessageStatus.Processing
        };
    }

    /// <summary>
    /// Помечает событие как успешно обработанное
    /// </summary>
    public void MarkCompleted()
    {
        Status = InboxMessageStatus.Completed;
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    /// <summary>
    /// Помечает событие как завершившееся ошибкой
    /// </summary>
    public void MarkFailed(string message)
    {
        Status = InboxMessageStatus.Failed;
        Error = message;
    }
}
