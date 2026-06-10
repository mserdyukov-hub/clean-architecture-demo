using Application.Common.Caching;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService)
    : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException("User not found", request.Id);

        user.UpdateProfile(request.UserName, request.FirstName, request.LastName);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // todo В дальнейшем уйдет из Handler
        await cacheService.RemoveAsync(CacheKeys.Users(), cancellationToken);
        await cacheService.RemoveAsync(CacheKeys.User(request.Id), cancellationToken);
    }
}
