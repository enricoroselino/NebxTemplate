using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Shared;
using Shared.Models.Exceptions;

namespace BuildingBlocks.API.Extensions;

public static class ClaimExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        // somehow the 'sub' is translated into ClaimsTypes.NameIdentifier
        var sub = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(sub, out var userId) || Guid.Empty.Equals(userId)) throw new UnauthorizedException();
        return userId;
    }

    public static Guid GetTokenId(this ClaimsPrincipal claimsPrincipal)
    {
        var jti = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (!Guid.TryParse(jti, out var tokenId) || Guid.Empty.Equals(tokenId)) throw new UnauthorizedException();
        return tokenId;
    }
    
    public static bool IsImpersonating(this ClaimsPrincipal user)
    {
        return user.HasClaim(c => c.Type == CustomClaim.ImpersonatorId);
    }
}