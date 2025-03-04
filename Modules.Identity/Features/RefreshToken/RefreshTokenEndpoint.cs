using BuildingBlocks.API.Configurations.Endpoint;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Identity.Constants;

namespace Modules.Identity.Features.RefreshToken;

public class RefreshTokenEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.Authentication.Route);

        group.MapPost("refresh", async (
                ISender mediator,
                HttpContext httpContext,
                ClaimsPrincipal principal,
                CancellationToken ct,
                [FromQuery] string refreshToken) =>
            {
                var userId = principal.GetUserId();
                var tokenId = principal.GetTokenId();
                if (tokenId is null || userId is null)
                {
                    return Verdict.Unauthorized().ToResult(httpContext);
                }

                var command = new RefreshTokenCommand(userId.Value, tokenId.Value, refreshToken);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(RefreshTokenEndpoint))
            .WithSummary("Refresh token session")
            .WithTags(ApiMeta.Authentication.Tag);
    }
}