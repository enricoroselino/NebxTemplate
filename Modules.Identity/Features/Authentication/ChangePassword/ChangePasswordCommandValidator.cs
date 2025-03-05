using System.Text.RegularExpressions;
using FluentValidation;
using Modules.Identity.Constants;

namespace Modules.Identity.Features.Authentication.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        var passwordRe = IdentityRegex.ValidPassword;

        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(ValidationConstant.MinPasswordLength)
            .MaximumLength(ValidationConstant.MaxPasswordLength)
            .Matches(passwordRe.Pattern, RegexOptions.Compiled)
            .WithMessage(passwordRe.Message);
        RuleFor(x => x.PasswordConfirmation)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Password confirmation must match the password.");
        RuleFor(x => x.TokenId).NotEqual(Guid.Empty);
    }
}