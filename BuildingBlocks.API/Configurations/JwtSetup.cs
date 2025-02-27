using System.Text;
using Ardalis.GuardClauses;
using BuildingBlocks.API.Extensions;
using BuildingBlocks.Contract.Infrastructure.JwtManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Exceptions;

namespace BuildingBlocks.API.Configurations;

public static class JwtSetup
{
    public static void AddJwtAuthenticationSetup(this IServiceCollection services)
    {
        services
            .AddOptions<JwtManagerOptions>()
            .BindConfiguration(JwtManagerOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IJwtManager, JwtManager>();

        using var serviceProvider = services.BuildServiceProvider();
        var tokenOptions = serviceProvider.GetRequiredService<IOptions<JwtManagerOptions>>().Value;

        var key = Encoding.UTF8.GetBytes(tokenOptions.Key);
        var symmetricKey = new SymmetricSecurityKey(key);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = tokenOptions.GracePeriodSpan,
            ValidIssuer = tokenOptions.ValidIssuer,
            ValidAudiences = tokenOptions.ValidAudiences,
            IssuerSigningKey = symmetricKey,
            ValidateTokenReplay = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        const int statusCode = StatusCodes.Status401Unauthorized;
                        var unauthorized = new UnauthorizedException();
                        var errorResponse = ErrorResponse.Create(unauthorized.Message, context.HttpContext, statusCode);
                        return context.Response.WriteAsJsonAsync(errorResponse);
                    },
                    OnForbidden = context =>
                    {
                        const int statusCode = StatusCodes.Status403Forbidden;
                        var forbidden = new ForbiddenException();
                        var errorResponse = ErrorResponse.Create(forbidden.Message, context.HttpContext, statusCode);
                        return context.Response.WriteAsJsonAsync(errorResponse);
                    }
                };
            });

        services.AddScoped<TokenValidationParameters>(_ => tokenValidationParameters);
        services.AddAuthorization();
    }
}