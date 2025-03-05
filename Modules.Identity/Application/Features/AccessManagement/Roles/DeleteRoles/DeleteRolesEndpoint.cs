using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.DeleteRoles;

public class DeleteRolesEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.AccessManagement.Route);

        group.MapDelete("roles/{roleId:guid}", async (
                ISender mediator,
                HttpContext httpContext,
                CancellationToken ct,
                [FromRoute] Guid roleId) =>
            {
                var command = new DeleteRolesCommand(roleId);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(DeleteRolesEndpoint))
            .WithSummary("Deletes a role")
            .WithTags(ApiMeta.AccessManagement.Tag)
            .RequireAuthorization();
    }
}