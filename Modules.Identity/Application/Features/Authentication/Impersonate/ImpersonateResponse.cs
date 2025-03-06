using BuildingBlocks.API.Services.JwtManager;

namespace Modules.Identity.Application.Features.Authentication.Impersonate;

public record ImpersonateResponse(TokenResult AccessToken, TokenResult RefreshToken);
