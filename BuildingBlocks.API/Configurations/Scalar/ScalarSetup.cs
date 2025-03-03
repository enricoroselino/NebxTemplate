﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace BuildingBlocks.API.Configurations.Scalar;

public static class ScalarSetup
{
    public static void AddScalarSetup(this IServiceCollection services)
    {
        services.AddOpenApi(options => { options.AddDocumentTransformer<SecurityTransformer>(); });
    }

    public static void UseScalarSetup(this WebApplication app)
    {
        var client = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.RestSharp);

        app.MapScalarApiReference(options =>
        {
            options.DefaultHttpClient = client;
            options.Authentication = new ScalarAuthenticationOptions
            {
                PreferredSecurityScheme = "Bearer"
            };

            options.DarkMode = true;
            options.HideModels = false;
            options.Layout = ScalarLayout.Modern;
            options.ShowSidebar = true;
        });
    }
}