using BuildingBlocks.API;
using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Configurations.Scalar;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultSetup();
builder.Services.AddLoggerSetup();

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

app.MapEndpoints();
await app.RunAsync();