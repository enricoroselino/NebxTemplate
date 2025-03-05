using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Features.Authentication.RefreshToken;

public record RefreshTokenResponse(TokenResult AccessToken, TokenResult RefreshToken);
