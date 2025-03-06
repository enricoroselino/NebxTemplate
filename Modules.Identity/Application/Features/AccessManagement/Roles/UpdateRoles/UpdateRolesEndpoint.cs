using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.UpdateRoles;

public class UpdateRolesEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.AccessManagement.Route);

        group.MapPut("roles", async (
                ISender mediator,
                HttpContext httpContext,
                CancellationToken ct,
                [FromBody] UpdateRolesRequest dto) =>
            {
                var command = new UpdateRolesCommand(dto.RoleId, dto.Key, dto.Description);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(UpdateRolesEndpoint))
            .WithSummary("Update roles")
            .WithTags(ApiMeta.AccessManagement.Tag)
            .RequireAuthorization();
    }
}