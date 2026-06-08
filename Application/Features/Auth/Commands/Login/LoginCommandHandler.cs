using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
            throw new NotFoundException("User not found", request.Email);

        // Проверка возможности логина
        user.EnsureLogin();

        // Проверка пароля
        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            user.RecordFailedLogin();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new ForbiddenException("login", "invalid password");
        }

        var token = jwtTokenGenerator.GenerateToken(user);

        user.RecordSuccessfulLogin();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            Token = token
        };
    }
}
