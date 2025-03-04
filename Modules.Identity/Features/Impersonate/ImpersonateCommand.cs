using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.Impersonate;

public record ImpersonateCommand(Guid TargetUserId, Guid ImpersonatorUserId, Guid ImpersonatorTokenId)
    : ICommand<Verdict<Response<ImpersonateResponse>>>;