using Microsoft.AspNetCore.Authorization;
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
        // check if the endpoint need authorization
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null ||
            endpoint?.Metadata.GetMetadata<IAuthorizeData>() is null)
        {
            await _next(context);
            return;
        }

        if (context.User.Identity is not ClaimsIdentity { IsAuthenticated: true })
        {
            await _next(context);
            return;
        }

        var jti = context.User.GetTokenId();
        var userId = context.User.GetUserId();
        if (jti is null || userId is null) throw new UnauthorizedException();

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