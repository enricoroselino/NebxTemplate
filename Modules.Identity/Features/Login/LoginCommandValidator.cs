using FluentValidation;

namespace Modules.Identity.Features.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.Identifier).NotEmpty();
        RuleFor(p => p.Password).NotEmpty();
    }
}