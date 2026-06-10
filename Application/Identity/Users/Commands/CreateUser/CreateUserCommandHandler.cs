using Application.Common.Caching;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Application.Identity.Users.Commands.CreateUser;

/// <summary>
/// Обработчик команды создания пользователя
/// </summary>
/// <param name="userRepository"></param>
/// <param name="unitOfWork"></param>
/// <param name="passwordHasher"></param>
public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ICacheService cacheService)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // проверка уникальности Email
        var email = Email.Create(request.Email);
        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
            throw new ConflictException("User", "email", request.Email);

        // Проверка уникальности Username
        if (await userRepository.ExistsByUsernameAsync(request.UserName, cancellationToken))
            throw new ConflictException("User", "username", request.UserName);

        // 3. Хеширование пароля
        var passwordHash = passwordHasher.Hash(request.Password);

        // 4. Создание пользователя
        var user = User.Create(request.UserName, email, passwordHash);

        // 5. Сохранение пользователя
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // todo В дальнейшем уйдет из Handler
        await cacheService.RemoveAsync(CacheKeys.Users(), cancellationToken);
        await cacheService.RemoveAsync(CacheKeys.User(user.Id), cancellationToken);

        return user.Id;
    }
}
