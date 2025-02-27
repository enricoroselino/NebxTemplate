using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace Shared.Configurations;

public static class Logger
{
    public static void Create(Action<LoggerConfiguration>? configurationBuilder = null)
    {
        var loggerConfiguration = new LoggerConfiguration();

        loggerConfiguration.Enrich.FromLogContext();

        loggerConfiguration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"));

        loggerConfiguration.WriteTo.Console();
        
        configurationBuilder?.Invoke(loggerConfiguration);
        Log.Logger = loggerConfiguration.CreateLogger();
    }
}