using Application.Common.Caching;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Identity.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException("User not found", request.Id);

        userRepository.Remove(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // todo В дальнейшем уйдет из Handler
        await cacheService.RemoveAsync(CacheKeys.Users(), cancellationToken);
        await cacheService.RemoveAsync(CacheKeys.User(request.Id), cancellationToken);
    }
}
