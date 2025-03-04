namespace Modules.Identity.Features.ImpersonateRevert;

public record ImpersonateRevertCommand(Guid TargetUserId, Guid TargetTokenId, Guid ImpersonatorUserId)
    : ICommand<Verdict<Response<ImpersonateRevertResponse>>>;