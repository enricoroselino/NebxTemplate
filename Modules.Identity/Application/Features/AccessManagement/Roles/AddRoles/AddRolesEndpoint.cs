using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.AddRoles;

public class AddRolesEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.AccessManagement.Route);

        group.MapPost("roles", async (
                ISender mediator,
                HttpContext httpContext,
                CancellationToken ct,
                [FromBody] AddRolesRequest dto) =>
            {
                var command = new AddRolesCommand(dto.Key, dto.Description);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(AddRolesEndpoint))
            .WithSummary("Add a new role")
            .WithTags(ApiMeta.AccessManagement.Tag)
            .RequireAuthorization();
    }
}