using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Application.Features.Authentication.ImpersonateRevert;

public record ImpersonateRevertResponse(TokenResult AccessToken, TokenResult RefreshToken);
