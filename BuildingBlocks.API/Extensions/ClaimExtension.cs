using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Shared;

namespace BuildingBlocks.API.Extensions;

public static class ClaimExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.Claims
            .Where(x => x.Type is ClaimTypes.NameIdentifier or JwtRegisteredClaimNames.Sub)
            .Select(x => x.Value)
            .FirstOrDefault();

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