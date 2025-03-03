using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.ImpersonateRevert;

public record ImpersonateRevertCommand(Guid ImpersonatorId) : ICommand<Verdict<Response<ImpersonateRevertResponse>>>;