using System.Text.RegularExpressions;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.Authentication.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        var usernameRe = IdentityRegex.ValidUserName;
        var passwordRe = IdentityRegex.ValidPassword;
        var fullnameRe = IdentityRegex.ValidFullName;
        
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(ValidationConstant.MinUsernameLength)
            .MaximumLength(ValidationConstant.MaxUsernameLength)
            .Matches(usernameRe.Pattern, RegexOptions.Compiled)
            .WithMessage(usernameRe.Message);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(ValidationConstant.MinPasswordLength)
            .MaximumLength(ValidationConstant.MaxPasswordLength)
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
            .MaximumLength(ValidationConstant.MaxNameLength)
            .Matches(fullnameRe.Pattern, RegexOptions.Compiled)
            .WithMessage(fullnameRe.Message);
    }
}