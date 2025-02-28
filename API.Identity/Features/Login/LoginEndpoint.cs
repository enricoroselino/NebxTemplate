using System.Security.Claims;
using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Services.JwtManager;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Models.Responses;

namespace API.Identity.Features.Login;

public class LoginEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (ISender mediator, CancellationToken ct) =>
            {
                var command = new LoginCommand();
                var response = await mediator.Send(command, ct);
                return response;
            })
            .WithName(nameof(LoginEndpoint))
            .WithTags("Dummy Endpoints");
    }
}