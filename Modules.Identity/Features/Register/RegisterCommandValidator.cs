using System.Text.RegularExpressions;
using FluentValidation;
using Modules.Identity.Constants;

namespace Modules.Identity.Features.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        var usernameRe = IdentityRegex.ValidUserName;
        var passwordRe = IdentityRegex.ValidPassword;
        var fullnameRe = IdentityRegex.ValidFullName;
        
        RuleFor(x => x.Username)
            .NotEmpty()
            .Matches(usernameRe.Pattern, RegexOptions.Compiled)
            .WithMessage(usernameRe.Message);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(passwordRe.Pattern, RegexOptions.Compiled)
            .WithMessage(passwordRe.Message);
        
        RuleFor(x => x.PasswordConfirmation)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Password confirmation must match the password.");;

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(x => x.Fullname)
            .NotEmpty()
            .Matches(fullnameRe.Pattern, RegexOptions.Compiled)
            .WithMessage(fullnameRe.Message);
    }
}