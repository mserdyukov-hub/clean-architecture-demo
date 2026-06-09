using Domain.Common;

namespace Domain.Aggregates.Identity;

/// <summary>
/// Возникает после успешного создания пользователя
///
/// Важно:
/// Это не команда создать пользователя
/// Это факт того, что пользователь уже создан
/// </summary>
/// <param name="Id">
/// Идентификатор созданного пользователя
/// </param>
/// <param name="Email">
/// Email созданного пользователя
/// </param>
public record UserCreatedDomainEvent(Guid Id, string Email) : IDomainEvent;
