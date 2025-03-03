using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Shared;
using Shared.Models.Exceptions;

namespace BuildingBlocks.API.Extensions;

public static class ClaimExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        // somehow the 'sub' is translated into ClaimsTypes.NameIdentifier
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(sub, out var userId) || Guid.Empty.Equals(userId)) return null;
        return userId;
    }

    public static Guid? GetTokenId(this ClaimsPrincipal user)
    {
        var jti = user.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (!Guid.TryParse(jti, out var tokenId) || Guid.Empty.Equals(tokenId)) return null;
        return tokenId;
    }

    public static bool IsImpersonating(this ClaimsPrincipal user)
    {
        return user.HasClaim(c => c.Type == CustomClaim.ImpersonatorId);
    }
    
    public static Guid? GetImpersonatorId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(CustomClaim.ImpersonatorId);
        if (!Guid.TryParse(sub, out var userId) || Guid.Empty.Equals(userId)) return null;
        return userId;
    }
}