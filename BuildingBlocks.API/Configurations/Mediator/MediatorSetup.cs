using System.Reflection;
using BuildingBlocks.API.Configurations.EFCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.API.Configurations.Mediator;

public static class MediatorSetup
{
    public static void AddMediatorSetup(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddInterceptors();
    }
}