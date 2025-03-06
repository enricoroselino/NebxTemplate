using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.Authentication.Register;

public class RegisterEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.Authentication.Route);
        
        group.MapPost("register", async (
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
            .WithName(nameof(RegisterEndpoint))
            .WithSummary("Register a new user")
            .WithTags(ApiMeta.Authentication.Tag);
    }
}