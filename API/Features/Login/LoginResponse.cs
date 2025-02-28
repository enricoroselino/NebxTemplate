using BuildingBlocks.API.Services.JwtManager;

namespace API.Features.Login;

public record LoginResponse(TokenResult AccessToken, TokenResult RefreshToken);
