using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Обработчик запроса получения данных пользователя и его ролей
/// </summary>
/// <param name="userRepository"></param>
public class GetUserByIdQueryHandler(IUserRepository userRepository, ICacheService cacheService) : IRequestHandler<GetUserByIdQuery, UserDetailDto>
{
    public async Task<UserDetailDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{request.Id}";

        var cachedUser =
            await cacheService.GetAsync<UserDetailDto>(
                cacheKey,
                cancellationToken);

        if (cachedUser is not null)
        {
            return cachedUser;
        }


        var user = await userRepository.GetDetailsByIdAsync(request.Id, cancellationToken);
        if (user is null)
            throw new NotFoundException("User not found", request.Id);

        var result = new UserDetailDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        };

        await cacheService.SetAsync(
            cacheKey,
            result,
            TimeSpan.FromMinutes(5),
            cancellationToken);

        return result;
    }
}
