﻿namespace Modules.Identity.Features.Authentication.ImpersonateRevert;

public record ImpersonateRevertCommand(Guid TargetUserId, Guid TargetTokenId, Guid ImpersonatorUserId)
    : ICommand<Verdict<Response<ImpersonateRevertResponse>>>;