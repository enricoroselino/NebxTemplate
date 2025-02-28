using BuildingBlocks.API.Services.JwtManager;

namespace API.Identity.Features.Login;

public record LoginResponse(TokenResult AccessToken, TokenResult RefreshToken);
