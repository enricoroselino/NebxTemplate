namespace Modules.Identity.Application.Features.Authentication.Impersonate;

public class ImpersonateCommandValidator : AbstractValidator<ImpersonateCommand>
{
    public ImpersonateCommandValidator()
    {
        RuleFor(x => x.TargetUserId).NotEqual(Guid.Empty);
        RuleFor(x => x.ImpersonatorUserId).NotEqual(Guid.Empty);
        RuleFor(x => x.ImpersonatorTokenId).NotEqual(Guid.Empty);
    }
}