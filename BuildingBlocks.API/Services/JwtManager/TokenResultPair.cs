namespace BuildingBlocks.API.Services.JwtManager;

public record TokenResultPair(TokenResult AccessToken, TokenResult RefreshToken);
