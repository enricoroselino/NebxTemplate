using System.Security.Claims;
using BuildingBlocks.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Modules.Identity.Domain.Services;

namespace Modules.Identity.Infrastructure;

public class ClaimsTransformationMiddleware
{
    private readonly RequestDelegate _next;
    private const string Prefix = nameof(ClaimsTransformationMiddleware);

    public ClaimsTransformationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IClaimServices claimServices,
        ILogger<ClaimsTransformationMiddleware> logger)
    {
        if (context.User.Identity is not ClaimsIdentity { IsAuthenticated: true })
        {
            await _next(context);
            return;
        }

        var userId = context.User.GetUserId();
        if (userId is null) throw new UnauthorizedAccessException();

        logger.LogDebug("[{Prefix}] Starting claims transformation for UserId: {UserId};", Prefix, userId);
        
        var claims = await claimServices.GetClaims(userId.Value);
        var retainClaims = context.User.Claims
            .Where(x => !claims.Select(y => y.Type).Contains(x.Type));

        var loadedClaims = retainClaims.Concat(claims);
        var newIdentity = new ClaimsIdentity(loadedClaims, JwtBearerDefaults.AuthenticationScheme);
        context.User = new ClaimsPrincipal(newIdentity);

        logger.LogDebug("[{Prefix}] Loaded {ClaimsCount} claims for UserId: {UserId};", Prefix, claims.Count, userId);
        await _next(context);
    }
}