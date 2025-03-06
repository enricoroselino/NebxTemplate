namespace Modules.Identity.Application.Features.Authentication.Impersonate;

public record ImpersonateCommand(Guid TargetUserId, Guid ImpersonatorUserId, Guid ImpersonatorTokenId)
    : ICommand<Verdict<Response<ImpersonateResponse>>>;