using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Features.Login;

public record LoginResponse(TokenResult AccessToken, TokenResult RefreshToken);
