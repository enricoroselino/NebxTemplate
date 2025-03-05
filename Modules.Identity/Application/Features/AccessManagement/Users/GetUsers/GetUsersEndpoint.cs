using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity.Constants;
using Shared.Models.ValueObjects;

namespace Modules.Identity.Application.Features.AccessManagement.Users.GetUsers;

public class GetUsersEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.AccessManagement.Route);

        group.MapGet("users", async (
                ISender mediator,
                CancellationToken ct,
                [FromQuery] string? searchTerm,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10) =>
            {
                var pagination = new Pagination(page, pageSize);

                var query = new GetUsersQuery(searchTerm?.Trim(), pagination);
                var result = await mediator.Send(query, ct);
                return result.ToResult();
            })
            .WithName(nameof(GetUsersEndpoint))
            .WithSummary("Get registered users")
            .WithTags(ApiMeta.AccessManagement.Tag)
            .RequireAuthorization();
    }
}