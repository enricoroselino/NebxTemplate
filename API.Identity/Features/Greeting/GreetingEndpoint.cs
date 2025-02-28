using BuildingBlocks.API.Configurations.Endpoint;
using MediatR;

namespace API.Identity.Features.Greeting;

public class GreetingEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/dummy/greeting", async (ISender mediator, CancellationToken ct) =>
            {
                var query = new GreetingQuery();
                var response = await mediator.Send(query, ct);
                return response;
            })
            .WithName(nameof(GreetingEndpoint))
            .WithTags("Dummy Endpoints")
            .RequireAuthorization();
    }
}