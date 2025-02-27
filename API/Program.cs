using BuildingBlocks.API;
using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Configurations.Scalar;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLoggerSetup();
builder.Services.AddScalarSetup();
builder.Services.AddJsonSetup();
builder.Services.AddIdempotentSetup();
builder.Services.AddJwtAuthenticationSetup();

builder.Services.AddCors();
builder.Services.AddAntiforgery();
builder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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