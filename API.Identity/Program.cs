using System.Security.Claims;
using BuildingBlocks.API;
using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.ApiDocumentation;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Services.JwtManager;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Models.Responses;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultSetup();
builder.Services.AddLoggerSetup();
builder.Services.AddJwtAuthenticationSetup();

builder.Services.AddModuleSetup(typeof(Program).Assembly);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseScalarSetup();
}

app.UseExceptionHandler(_ => { });

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/hello", () => Response.Build("Harimau Melaya"))
    .RequireAuthorization();

app.MapGet("/login", (IJwtManager jwtManager) =>
{
    var claims = new List<Claim>()
    {
        new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, "Suka Tani"),
        new Claim(JwtRegisteredClaimNames.Email, "bayarpolisi@gmail.com")
    };

    var accessToken = jwtManager.CreateAccessToken(claims);
    var refreshToken = jwtManager.CreateRefreshToken();

    return Response.Build(new
    {
        Accesstoken = accessToken,
        RefreshToken = refreshToken
    });
});

app.MapEndpoints();
await app.RunAsync();