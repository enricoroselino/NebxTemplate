using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.API.Configurations;

internal static class JsonSetup
{
    public static void AddJsonSetup(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            // change enums into its string representation globally
            // same thing as [JsonConverter(typeof(JsonStringEnumConverter))]
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}