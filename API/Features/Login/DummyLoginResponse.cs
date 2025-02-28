using BuildingBlocks.API.Services.JwtManager;

namespace API.Features.Login;

public record DummyLoginResponse(TokenResult AccessToken, TokenResult RefreshToken);
