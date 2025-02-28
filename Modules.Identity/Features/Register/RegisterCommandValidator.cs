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
            .NotEmpty();
        
        RuleFor(x => x.Email)
            .NotEmpty();
        
        RuleFor(x => x.Fullname)
            .NotEmpty();
    }
}