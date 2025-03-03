using FluentValidation;

namespace Modules.Identity.Features.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();
        
        RuleFor(x => x.Password)
            .NotEmpty();
        
        RuleFor(x => x.PasswordConfirmation)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Password confirmation must match the password.");;
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();;
        
        RuleFor(x => x.Fullname)
            .NotEmpty();
    }
}