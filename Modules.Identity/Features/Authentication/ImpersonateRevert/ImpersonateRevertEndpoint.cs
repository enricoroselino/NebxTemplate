using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;

namespace Modules.Identity.Features.Authentication.ImpersonateRevert;

// TODO : make sure only admin can access

public class ImpersonateRevertEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.Authentication.Route);

        group.MapPost("impersonate/revert", async (
                ISender mediator,
                HttpContext httpContext,
                ClaimsPrincipal principal,
                CancellationToken ct) =>
            {
                var impersonatorResult = ValidateImpersonator(principal);
                if (!impersonatorResult.IsSuccess) return impersonatorResult.ToResult(httpContext);

                var impersonatorUserId = impersonatorResult.Value;
                
                var targetTokenId = principal.GetTokenId();
                var targetUserId = principal.GetUserId();
                if (targetTokenId is null || targetUserId is null)
                {
                    return Verdict.Unauthorized().ToResult(httpContext);
                } 

                var command = new ImpersonateRevertCommand(
                    targetUserId.Value, 
                    targetTokenId.Value, 
                    impersonatorUserId);
                
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(ImpersonateRevertEndpoint))
            .WithTags(ApiMeta.Authentication.Tag)
            .WithSummary("Exit impersonate mode")
            .RequireAuthorization();
    }

    private static Verdict<Guid> ValidateImpersonator(ClaimsPrincipal principal)
    {
        if (!principal.IsImpersonating())
        {
            return Verdict.BadRequest("Not currently impersonating an user");
        }

        var impersonatorId = principal.GetImpersonatorUserId();
        return impersonatorId is null ? Verdict.Unauthorized() : Verdict.Success(impersonatorId.Value);
    }
}