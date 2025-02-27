using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations.Helper;

namespace BuildingBlocks.API.Configurations.Endpoint;

public static class EndpointSetup
{
    internal static void AddEndpointSetup(this IServiceCollection services, Assembly assembly)
    {
        var types = AssembliesHelper.GetInterfaceTypes<IEndpoint>(assembly);
        var descriptors = types.Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));
        services.TryAddEnumerable(descriptors);
    }

    public static void MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEndpointRouteBuilder routeBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;

        var endpoints = app.Services
            .GetRequiredService<IEnumerable<IEndpoint>>()
            .ToList();

        endpoints.ForEach(x => x.AddRoutes(routeBuilder));
    }
}