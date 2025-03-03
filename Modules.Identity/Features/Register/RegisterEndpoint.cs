using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Modules.Identity.Features.Register;

public class RegisterEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/identity/register", async (
                ISender mediator,
                HttpContext httpContext,
                CancellationToken ct,
                [FromBody] RegisterRequest dto) =>
            {
                var command = new RegisterCommand(
                    dto.Username,
                    dto.Password,
                    dto.PasswordConfirmation,
                    dto.Email,
                    dto.FullName);

                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithTags("Identity");
    }
}