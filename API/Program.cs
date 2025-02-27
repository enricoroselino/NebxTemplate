using BuildingBlocks.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddLoggerSetup();
builder.Services.AddJsonSetup();
builder.Services.AddIdempotentSetup();
builder.Services.AddJwtAuthenticationSetup();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();