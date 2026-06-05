using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand: IRequest
{
    public Guid Id { get; set; }

    public string UserName { get; init; }
    
    public string FirstName { get; init; }
    
    public string LastName { get; init; }
}