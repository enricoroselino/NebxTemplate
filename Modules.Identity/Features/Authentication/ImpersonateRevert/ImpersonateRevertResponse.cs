using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Features.Authentication.ImpersonateRevert;

public record ImpersonateRevertResponse(TokenResult AccessToken, TokenResult RefreshToken);
