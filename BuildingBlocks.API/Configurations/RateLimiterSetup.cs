using BuildingBlocks.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.API.Configurations;

public static class RateLimiterSetup
{
    public static void AddRateLimiterSetup(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            const int statusCode = StatusCodes.Status429TooManyRequests;
            options.RejectionStatusCode = statusCode;

            options.OnRejected = (context, token) =>
            {
                const string message = "You have exceeded the allowed request limit, please try again later.";
                var errorResponse = ErrorResponse.Create(message, context.HttpContext, statusCode);
                context.HttpContext.Response.WriteAsJsonAsync(errorResponse, token).GetAwaiter().GetResult();
                return ValueTask.CompletedTask;
            };
        });
    }
}