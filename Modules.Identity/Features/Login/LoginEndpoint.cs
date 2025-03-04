using BuildingBlocks.API.Configurations.Endpoint;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Identity.Constants;

namespace Modules.Identity.Features.Login;

public class LoginEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.Authentication.Route);

        group.MapPost("login", async (
                ISender mediator,
                HttpContext httpContext,
                CancellationToken ct,
                [FromBody] LoginRequest dto) =>
            {
                var command = new LoginCommand(dto.Identifier, dto.Password);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(LoginEndpoint))
            .WithSummary("Login to get JWT")
            .WithTags(ApiMeta.Authentication.Tag);
    }
}