using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Features.RefreshToken;

public record RefreshTokenResponse(TokenResult AccessToken, TokenResult RefreshToken);
