using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Features.Authentication.Login;

public record LoginResponse(TokenResult AccessToken, TokenResult RefreshToken);
