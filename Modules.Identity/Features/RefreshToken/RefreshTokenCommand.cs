using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.RefreshToken;

public record RefreshTokenCommand(Guid UserId, Guid TokenId, string RefreshToken)
    : ICommand<Verdict<Response<RefreshTokenResponse>>>;