using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.Impersonate;

public record ImpersonateCommand(Guid UserId, Guid CurrentUserId) : ICommand<Verdict<Response<ImpersonateResponse>>>;
