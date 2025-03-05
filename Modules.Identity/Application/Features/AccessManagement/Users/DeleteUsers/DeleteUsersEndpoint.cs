using BuildingBlocks.API.Configurations.Endpoint;
using Microsoft.AspNetCore.Mvc;
using Modules.Identity.Constants;

namespace Modules.Identity.Application.Features.AccessManagement.Users.DeleteUsers;

public class DeleteUsersEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.AccessManagement.Route);

        group.MapDelete("users/{userId:guid}", async (
                ISender mediator,
                HttpContext httpContext,
                CancellationToken ct,
                [FromRoute] Guid userId) =>
            {
                var command = new DeleteUsersCommand(userId);
                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(DeleteUsersEndpoint))
            .WithSummary("Deactivate users")
            .WithTags(ApiMeta.AccessManagement.Tag)
            .RequireAuthorization();
    }
}