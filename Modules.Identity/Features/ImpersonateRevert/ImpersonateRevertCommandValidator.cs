using FluentValidation;

namespace Modules.Identity.Features.ImpersonateRevert;

public class ImpersonateRevertCommandValidator : AbstractValidator<ImpersonateRevertCommand>
{
    public ImpersonateRevertCommandValidator()
    {
        RuleFor(x => x.ImpersonatorUserId).NotEmpty();
        RuleFor(x => x.TargetUserId).NotEmpty();
        RuleFor(x => x.TargetTokenId).NotEmpty();
    }
}