using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Application.Features.Authentication.RefreshToken;

public record RefreshTokenResponse(TokenResult AccessToken, TokenResult RefreshToken);
