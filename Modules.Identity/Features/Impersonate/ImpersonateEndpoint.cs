﻿using System.Security.Claims;
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

// TODO : make sure only admin can access

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
                [FromQuery] Guid targetUserId) =>
            {
                var validateResult = ValidateUser(principal, targetUserId);
                if (!validateResult.IsSuccess) return validateResult.ToResult(httpContext);

                var command = new ImpersonateCommand(targetUserId, validateResult.Value);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(ImpersonateEndpoint))
            .WithTags(ApiMeta.Authentication.Tag)
            .WithSummary("Get JWT of the impersonated user")
            .RequireAuthorization();
    }

    private static Verdict<Guid> ValidateUser(ClaimsPrincipal principal, Guid targetUserId)
    {
        if (principal.IsImpersonating())
        {
            return Verdict.BadRequest("Already in impersonation mode");
        }

        var currentUser = principal.GetUserId();
        if (currentUser == targetUserId)
        {
            return Verdict.BadRequest("You cannot impersonate yourself");
        }

        if (currentUser is null) throw new UnauthorizedException();

        return Verdict.Success(currentUser.Value);
    }
}