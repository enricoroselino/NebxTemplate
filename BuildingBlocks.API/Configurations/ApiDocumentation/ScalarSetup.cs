using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace BuildingBlocks.API.Configurations.ApiDocumentation;

public static class ScalarSetup
{
    public static void AddScalarSetup(this IServiceCollection services)
    {
        services.AddOpenApi(options => { options.AddDocumentTransformer<OpenApiSecurityTransformer>(); });
    }

    public static void UseScalarSetup(this WebApplication app)
    {
        var client = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.RestSharp);

        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.DefaultHttpClient = client;
            options.Authentication = new ScalarAuthenticationOptions
            {
                PreferredSecurityScheme = JwtBearerDefaults.AuthenticationScheme
            };

            options.DarkMode = true;
            options.HideModels = true;
            options.Layout = ScalarLayout.Modern;
            options.ShowSidebar = true;
        });
    }
}