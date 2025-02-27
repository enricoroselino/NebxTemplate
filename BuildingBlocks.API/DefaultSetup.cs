using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.ApiDocumentation;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.API;

public static class DefaultSetup
{
    public static void AddDefaultSetup(this IServiceCollection services)
    {
        services.AddJwtAuthenticationSetup();
        
        services.AddScalarSetup();
        services.AddJsonSetup();
        services.AddIdempotentSetup();

        services.AddCors();
        services.AddAntiforgery();
        services.AddSingleton<TimeProvider>(_ => TimeProvider.System);

        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}