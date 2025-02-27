using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BuildingBlocks.API.Configurations.EFCore;

internal static class InterceptorSetup
{
    public static void AddInterceptors(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
    }
}