using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;

namespace API.Features.Greeting;

public class GreetingQueryHandler : IQueryHandler<GreetingQuery, Response<string>>
{
    public Task<Response<string>> Handle(GreetingQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Response.Build("Harimau Melaya"));
    }
}