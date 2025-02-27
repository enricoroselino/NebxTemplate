using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.API.Configurations.Endpoint;

public interface IEndpoint
{
    void AddRoutes(IEndpointRouteBuilder app);
}