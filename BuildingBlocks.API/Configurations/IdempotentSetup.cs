using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using IdempotentAPI.Core;
using IdempotentAPI.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.API.Configurations;

/// <summary>
/// Provides an extension method to configure and add idempotency support to the application's services.
/// </summary>
public static class IdempotentSetup
{
    /// <summary>
    /// Adds idempotency support to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the idempotency services to.</param>
    /// <remarks>
    /// This method configures the services required to ensure that HTTP POST and PATCH requests
    /// are processed only once for a given request data and idempotency key. It utilizes the
    /// IdempotentAPI library to achieve this functionality.
    /// 
    /// For more information and detailed documentation, please refer to the
    /// <see href="https://github.com/ikyriak/IdempotentAPI">IdempotentAPI GitHub repository</see>.
    /// </remarks>
    public static void AddIdempotentSetup(this IServiceCollection services)
    {
        services.AddIdempotentMinimalAPI(new IdempotencyOptions()
        {
            IsIdempotencyOptional = true
        });

        services.AddDistributedMemoryCache();
        services.AddIdempotentAPIUsingDistributedCache();
    }
}