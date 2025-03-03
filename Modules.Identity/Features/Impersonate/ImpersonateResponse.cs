using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Features.Impersonate;

public record ImpersonateResponse(TokenResult AccessToken, TokenResult RefreshToken);
