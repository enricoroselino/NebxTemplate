using FluentValidation;

namespace Modules.Identity.Features.ImpersonateRevert;

public class ImpersonateRevertCommandValidator : AbstractValidator<ImpersonateRevertCommand>
{
    public ImpersonateRevertCommandValidator()
    {
        RuleFor(x => x.ImpersonatorId).NotEmpty();
    }
}