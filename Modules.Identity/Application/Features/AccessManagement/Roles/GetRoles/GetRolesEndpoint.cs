using BuildingBlocks.API.Configurations.Endpoint;
using Microsoft.AspNetCore.Mvc;
using Modules.Identity.Constants;
using Shared.Models.ValueObjects;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.GetRoles;

public class GetRolesEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.AccessManagement.Route);

        group.MapGet("roles", async (
                ISender mediator,
                CancellationToken ct,
                [FromQuery] string? searchTerm,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10) =>
            {
                var pagination = new Pagination(page, pageSize);

                var query = new GetRolesQuery(searchTerm?.Trim(), pagination);
                var response = await mediator.Send(query, ct);
                return response.ToResult();
            })
            .WithName(nameof(GetRolesEndpoint))
            .WithSummary("Returns a list of roles from the database.")
            .WithTags(ApiMeta.AccessManagement.Tag);
    }
}