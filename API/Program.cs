using BuildingBlocks.API;
using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Configurations.Mediator;
using BuildingBlocks.API.Configurations.Scalar;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLoggerSetup();
builder.Services.AddScalarSetup();
builder.Services.AddJsonSetup();
builder.Services.AddIdempotentSetup();
builder.Services.AddJwtAuthenticationSetup();

builder.Services.AddMediatorSetup(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddEndpointSetup(typeof(Program).Assembly);

builder.Services.AddCors();
builder.Services.AddAntiforgery();
builder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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