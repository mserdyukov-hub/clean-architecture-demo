using FluentValidation;

namespace Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .WithMessage("Username must be between 3 and 50 characters");
        
        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters");
     
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email must be a valid email address");
    }
}