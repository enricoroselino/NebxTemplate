using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;

namespace API.Identity.Features.Greeting;

public class GreetingQueryHandler : IQueryHandler<GreetingQuery, Response<string>>
{
    public async Task<Response<string>> Handle(GreetingQuery request, CancellationToken cancellationToken)
    {
        return Response.Build("Harimau Melaya");
    }
}