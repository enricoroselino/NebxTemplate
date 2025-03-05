using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.Authentication.RefreshToken;

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