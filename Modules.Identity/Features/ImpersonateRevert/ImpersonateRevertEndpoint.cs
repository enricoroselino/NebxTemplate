using System.Security.Claims;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Identity.Constants;
using Shared.Models.Exceptions;
using Shared.Verdict;

namespace Modules.Identity.Features.ImpersonateRevert;

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
                var validateResult = ValidateUser(principal);
                if (!validateResult.IsSuccess) return validateResult.ToResult(httpContext);

                var command = new ImpersonateRevertCommand(validateResult.Value);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(ImpersonateRevertEndpoint))
            .WithTags(ApiMeta.Authentication.Tag)
            .WithSummary("Exit impersonate mode")
            .RequireAuthorization();
    }

    private static Verdict<Guid> ValidateUser(ClaimsPrincipal principal)
    {
        if (!principal.IsImpersonating())
        {
            return Verdict.BadRequest("Not currently impersonating an user");
        }

        var impersonatorId = principal.GetImpersonatorId();
        if (impersonatorId is null) throw new UnauthorizedException();
        return Verdict.Success(impersonatorId.Value);
    }
}