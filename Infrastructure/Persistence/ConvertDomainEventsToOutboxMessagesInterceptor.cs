using System.Text.Json;
using Application.Common.Interfaces;
using Domain.Aggregates.Identity;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Persistence;

/// <summary>
/// Перехватывает SaveChanges и преобразует доменные события в записи OutboxMessage
/// Благодаря этому агрегат и Outbox сохраняются в рамках одной транзакции БД.
/// </summary>
public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Вызывается EF Core непосредственно перед сохранением изменений
    /// На этом этапе извлекаем все Domain Events из агрегатов и создаём соответствующие записи OutboxMessage
    /// </summary>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        AddOutboxMessages(dbContext);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Извлекает все доменные события из отслеживаемых агрегатов, сериализует их и добавляет в таблицу Outbox
    /// </summary>
    private static void AddOutboxMessages(DbContext dbContext)
    {
        // Получаем все доменные события из агрегатов, которые сейчас отслеживает EF Core
        var domainEvents = dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregate =>
                {
                    // Копируем события во временный список.
                    var events = aggregate.DomainEvents.ToList(); // todo почему нет событий?

                    // Очищаем агрегат, чтобы события не были обработаны повторно
                    aggregate.ClearDomainEvents();

                    return events;
                }
            ).ToList();

        // Получаем фабрику интеграционных событий
        var factory = dbContext.GetService<IIntegrationEventFactory>();

        // Domain → Integration
        var integrationEvents = factory.Create(domainEvents);

        // Преобразуем каждое доменное событие в отдельную запись OutboxMessage
        var outboxMessages = integrationEvents
            .Select(integrationEvent => OutboxMessage.Create(
                evenId: integrationEvent.EventId,
                occurredOnUtc: integrationEvent.OccurredOnUtc,
                topic: ResolveTopic(integrationEvent),

                // Сохраняем полный тип события,чтобы позднее можно было восстановить объект
                eventType: integrationEvent.GetType().FullName!,

                // Сериализуем событие в JSON
                payload: JsonSerializer.Serialize(integrationEvent)
            ))
            .ToList();

        // Добавляем сообщения в контекст.
        // Они будут сохранены вместе с основными изменениями в рамках одной транзакции
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    // todo сделать отдельный TopicResolver
    private static string ResolveTopic(object integrationEvent)
    {
        return integrationEvent switch
        {
            UserCreatedDomainEvent => "users-topic",

            //OrderCreatedDomainEvent => "orders-topic",

            //ProductCreatedDomainEvent => "products-topic",

            _ => throw new InvalidOperationException(
                $"Topic mapping not found for {integrationEvent.GetType().Name}")
        };
    }
}
