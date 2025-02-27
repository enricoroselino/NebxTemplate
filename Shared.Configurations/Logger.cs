using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace Shared.Configurations;

/// <summary>
/// Provides a helper class to create and configure an ILogger instance.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Creates and configures a Serilog ILogger instance.
    /// </summary>
    /// <param name="configuration">
    /// An optional action to customize the logger configuration.
    /// </param>
    /// <returns>An instance of ILogger with the specified configuration.</returns>
    public static ILogger Create(Action<LoggerConfiguration>? configuration = null)
    {
        var loggerConfiguration = new LoggerConfiguration();

        loggerConfiguration.Enrich.FromLogContext();

        loggerConfiguration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"));

        loggerConfiguration.WriteTo.Console();
        
        configuration?.Invoke(loggerConfiguration);
        return Log.Logger = loggerConfiguration.CreateLogger();
    }
}