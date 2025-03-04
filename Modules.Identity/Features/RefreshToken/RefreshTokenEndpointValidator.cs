using FluentValidation;

namespace Modules.Identity.Features.RefreshToken;

public class RefreshTokenEndpointValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenEndpointValidator()
    {
        RuleFor(x => x.TokenId).NotEqual(Guid.Empty);
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}