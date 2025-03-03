using System.Security.Claims;
using BuildingBlocks.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
        ILogger<ClaimsTransformationMiddleware> logger)
    {
        if (context.User.Identity is not ClaimsIdentity { IsAuthenticated: true })
        {
            await _next(context);
            return;
        }

        var userId = context.User.GetUserId();

        logger.LogDebug("[{Prefix}] Starting claims transformation for UserId: {UserId};",
            Prefix, userId);

        var claimsDto = new List<Claim>();

        if (claimsDto.Count == 0)
        {
            await _next(context);
            return;
        }

        var toAddClaims = claimsDto
            .Select(x => new Claim(x.Type, x.Value))
            .Except(context.User.Claims)
            .ToList();

        var newIdentity = new ClaimsIdentity(context.User.Claims, JwtBearerDefaults.AuthenticationScheme);
        newIdentity.AddClaims(toAddClaims);

        context.User = new ClaimsPrincipal(newIdentity);

        logger.LogDebug("[{Prefix}] Loaded {ClaimsCount} claims for UserId: {UserId};",
            Prefix, toAddClaims.Count, userId);
        await _next(context);
    }
}