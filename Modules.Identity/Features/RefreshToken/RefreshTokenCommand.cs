namespace Modules.Identity.Features.RefreshToken;

public record RefreshTokenCommand(Guid UserId, Guid TokenId, string RefreshToken)
    : ICommand<Verdict<Response<RefreshTokenResponse>>>;