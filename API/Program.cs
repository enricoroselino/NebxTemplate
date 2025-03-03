using BuildingBlocks.API;
using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Configurations.Endpoint;
using Modules.Identity;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
var dir = Directory.GetCurrentDirectory();
builder.Configuration
    .SetBasePath(dir)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDefaultSetup();
builder.Services.AddLoggerSetup();
builder.Services.AddJwtAuthenticationSetup();

builder.Services.AddModuleSetup(typeof(Program).Assembly);

builder.Services.AddIdentityModule();

builder.WebHost.ConfigureKestrel(options =>
{
    // based on browser timeout
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseDefaultSetup();

app.UseAuthentication();
app.UseIdentityModuleMiddlewares();
app.UseAuthorization();

app.UseIdentityModule();

app.MapEndpoints();
await app.RunAsync();