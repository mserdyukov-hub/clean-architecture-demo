using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) :IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException("User not found", request.Id);
        
        user.UpdateProfile(request.UserName, request.FirstName, request.LastName);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}