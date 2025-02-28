using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.ApiDocumentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.API;

public static class DefaultSetup
{
    public static void AddDefaultSetup(this IServiceCollection services)
    {
        services.AddScalarSetup();
        services.AddSwaggerSetup();
        services.AddJsonSetup();
        services.AddRateLimiterSetup();

        services.AddCors();
        services.AddAntiforgery();

        services.AddSingleton<TimeProvider>(_ => TimeProvider.System);

        services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void UseDefaultSetup(this WebApplication app)
    {
        var useHttps = app.Configuration.GetValue<bool>("EnvironmentOptions:UseHttps");

        if (!app.Environment.IsProduction())
        {
            app.UseSwaggerSetup();
            app.UseScalarSetup();
        }

        if (useHttps) app.UseHttpsRedirection();
        app.UseExceptionHandler(_ => { });
    }
}