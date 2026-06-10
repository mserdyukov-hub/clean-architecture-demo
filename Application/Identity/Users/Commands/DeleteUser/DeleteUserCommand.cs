using MediatR;

namespace Application.Identity.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest;
