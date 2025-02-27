using System.Reflection;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Configurations.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.API.Configurations;

public static class ModuleSetup
{
    public static void AddModuleSetup(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatorSetup(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddEndpointSetup(assembly);
    }
}