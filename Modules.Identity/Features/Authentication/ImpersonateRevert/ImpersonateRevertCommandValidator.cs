using FluentValidation;

namespace Modules.Identity.Features.Authentication.ImpersonateRevert;

public class ImpersonateRevertCommandValidator : AbstractValidator<ImpersonateRevertCommand>
{
    public ImpersonateRevertCommandValidator()
    {
        RuleFor(x => x.ImpersonatorUserId).NotEqual(Guid.Empty);
        RuleFor(x => x.TargetUserId).NotEqual(Guid.Empty);
        RuleFor(x => x.TargetTokenId).NotEqual(Guid.Empty);
    }
}