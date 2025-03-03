using FluentValidation;

namespace Modules.Identity.Features.Impersonate;

public class ImpersonateCommandValidator : AbstractValidator<ImpersonateCommand>
{
    public ImpersonateCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ImpersonatorId).NotEmpty();
    }
}