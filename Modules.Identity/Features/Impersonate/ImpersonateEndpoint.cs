using System.Security.Claims;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Identity.Constants;
using Shared.Models.Exceptions;
using Shared.Verdict;

namespace Modules.Identity.Features.Impersonate;

public class ImpersonateEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.Authentication.Route);

        group.MapPost("impersonate", async (
                ISender mediator,
                HttpContext httpContext,
                ClaimsPrincipal principal,
                CancellationToken ct,
                [FromQuery] Guid userId) =>
            {
                if (principal.IsImpersonating())
                {
                    return Verdict
                        .BadRequest("Already in impersonation mode")
                        .ToResult(httpContext);
                }

                var impersonatorId = principal.GetUserId();
                if (impersonatorId == userId)
                {
                    return Verdict
                        .BadRequest("You cannot impersonate yourself")
                        .ToResult(httpContext);
                }

                if (impersonatorId is null) throw new UnauthorizedException();
                
                var command = new ImpersonateCommand(userId, impersonatorId.Value);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .RequireAuthorization();
    }
}