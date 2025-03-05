using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;
using Shared.Models.Exceptions;

namespace Modules.Identity.Features.GetClaims;

public class GetClaimsQueryEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.Authentication.Route);

        group.MapGet("claims", async (
                ISender mediator,
                ClaimsPrincipal principal,
                CancellationToken ct) =>
            {
                var userId = principal.GetUserId();
                if (userId is null) throw new UnauthorizedException();

                var query = new GetClaimsQuery(userId.Value);
                var response = await mediator.Send(query, ct);
                return response.ToResult();
            })
            .WithName(nameof(GetClaimsQueryEndpoint))
            .WithSummary("Get user claims")
            .WithTags(ApiMeta.Authentication.Tag)
            .RequireAuthorization();
    }
}