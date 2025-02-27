using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Shared.Configurations;

namespace BuildingBlocks.API;

/// <summary>
/// Provides an extension method to configure and add Serilog logging to the application.
/// </summary>
public static class LoggerSetup
{
    /// <summary>
    /// Configures and adds a Serilog logger to the application's service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the logger to.</param>
    /// <param name="configureLogger">
    /// An optional delegate that allows additional configuration of the <see cref="LoggerConfiguration"/>.
    /// This can be used to add custom sinks, enrich, and other logging settings.
    /// </param>
    public static void AddLoggerSetup(
        this IServiceCollection services,
        Action<LoggerConfiguration>? configureLogger = null)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        
        var logger = Logger.Create(configuration =>
        {
            configuration.MinimumLevel.Is(environment.IsProduction() ? LogEventLevel.Warning : LogEventLevel.Debug);
            
            configuration
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"));
            
            configuration
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithCorrelationId();
            
            // Allow projects to add their own sinks/configurations
            configureLogger?.Invoke(configuration);
        });

        services.AddSerilog(logger);
    }
}