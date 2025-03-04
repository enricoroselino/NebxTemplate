using System.Security.Claims;
using BuildingBlocks.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Identity.Data;
using Shared;
using Shared.Models.Exceptions;

namespace Modules.Identity.Infrastructure;

/// <summary>
/// a middleware to block a revoked token request
/// </summary>
public class RevokedTokenMiddleware
{
    private readonly RequestDelegate _next;
    private const string Prefix = nameof(RevokedTokenMiddleware);

    public RevokedTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        AppIdentityDbContext dbContext,
        TimeProvider timeProvider,
        ILogger<RevokedTokenMiddleware> logger)
    {
        // check if in impersonating mode
        var isImpersonating = context.User.Claims.Any(x => x.Type == CustomClaim.ImpersonatorId);
        if (isImpersonating)
        {
            await _next(context);
            return;
        }

        // check if the endpoint need authorization
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null ||
            endpoint?.Metadata.GetMetadata<IAuthorizeData>() is null)
        {
            await _next(context);
            return;
        }

        var jti = context.User.GetTokenId();
        if (jti is null) throw new UnauthorizedException();

        var userId = context.User.GetUserId();
        if (userId is null) throw new UnauthorizedException();

        if (context.User.Identity is not ClaimsIdentity { IsAuthenticated: true })
        {
            await _next(context);
            return;
        }

        var token = await dbContext.JwtStores
            .AsNoTracking()
            .Where(c => c.TokenId == jti && c.UserId == userId)
            .SingleOrDefaultAsync();

        var currentTime = timeProvider.GetUtcNow().DateTime;
        if (token is null || token.RevokedOn is not null || currentTime >= token.ExpiresOn)
        {
            logger.LogDebug("[{Prefix}] Token JTI: {Jti} is not valid. Rejecting request;", Prefix, jti);
            throw new UnauthorizedException();
        }

        logger.LogDebug("[{Prefix}] Token JTI: {Jti} is valid. Proceeding with request;", Prefix, jti);
        await _next(context);
    }
}