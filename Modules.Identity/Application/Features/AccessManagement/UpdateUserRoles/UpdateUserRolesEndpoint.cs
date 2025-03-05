using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.AccessManagement.UpdateUserRoles;

public class UpdateUserRolesEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.AccessManagement.Route);

        group.MapPut("roles/assign", async (
                ISender mediator,
                HttpContext httpContext,
                CancellationToken ct,
                [FromBody] UpdateUserRolesRequest dto) =>
            {
                var command = new UpdateUserRolesCommand(dto.UserId, dto.RoleIds);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(UpdateUserRolesEndpoint))
            .WithSummary("Updates roles associated with users.")
            .WithTags(ApiMeta.AccessManagement.Tag)
            .RequireAuthorization();
    }
}