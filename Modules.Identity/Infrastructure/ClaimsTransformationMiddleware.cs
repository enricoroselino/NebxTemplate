using System.Security.Claims;
using BuildingBlocks.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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

        logger.LogDebug("[{Prefix}] Starting claims transformation for UserId: {UserId};",
            Prefix, userId);

        var claims = await claimServices.GetClaims(userId.Value);
        if (claims.Count == 0)
        {
            await _next(context);
            return;
        }

        var newIdentity = new ClaimsIdentity(context.User.Claims, JwtBearerDefaults.AuthenticationScheme);
        newIdentity.AddClaims(claims);

        context.User = new ClaimsPrincipal(newIdentity);

        logger.LogDebug("[{Prefix}] Loaded {ClaimsCount} claims for UserId: {UserId};",
            Prefix, claims.Count, userId);
        await _next(context);
    }
}