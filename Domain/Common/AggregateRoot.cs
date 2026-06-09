namespace Domain.Common;

/// <summary>
/// Базовый класс для всех корней агрегатов
/// Хранит доменные события, произошедшие внутри агрегата
/// </summary>
/// <typeparam name="TKey">Тип идентификатора сущности</typeparam>
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
{
    /// <summary>
    /// Внутренний список доменных событий
    /// События добавляются агрегатом и публикуются после SaveChanges()
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Коллекция доменных событий только для чтения
    /// Нужна Infrastructure слою для публикации событий через MediatR
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    /// <summary>
    /// Добавляет новое доменное событие в очередь публикации
    /// </summary>
    /// <param name="domainEvent">
    /// </param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Очищает список событий после их публикации
    /// Вызывается Infrastructure слоем после MediatR.Publish()
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
