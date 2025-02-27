using BuildingBlocks.API;
using Serilog;
using Serilog.Events;
using Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddLoggerSetup();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.RunAsync();