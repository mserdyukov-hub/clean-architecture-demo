using MediatR;

namespace Application.Features.Users.Queries.GetUserById;
/// <summary>
/// Запрос для получения данных пользователя и его ролях
/// </summary>
/// <param name="Id"></param>
public record GetUserByIdQuery(Guid Id) : IRequest<UserDetailDto>;