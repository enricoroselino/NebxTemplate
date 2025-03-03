using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Features.ImpersonateRevert;

public record ImpersonateRevertResponse(TokenResult AccessToken, TokenResult RefreshToken);
