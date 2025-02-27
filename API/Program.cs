using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.Scalar;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScalarSetup();
builder.Services.AddLoggerSetup();
builder.Services.AddJsonSetup();
builder.Services.AddIdempotentSetup();
builder.Services.AddJwtAuthenticationSetup();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseScalarSetup();
}

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();