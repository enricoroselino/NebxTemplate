using FluentValidation;

namespace Modules.Identity.Features.Impersonate;

public class ImpersonateCommandValidator : AbstractValidator<ImpersonateCommand>
{
    public ImpersonateCommandValidator()
    {
        RuleFor(x => x.TargetUserId).NotEmpty();
        RuleFor(x => x.ImpersonatorUserId).NotEmpty();
    }
}