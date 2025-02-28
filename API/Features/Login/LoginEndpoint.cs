using BuildingBlocks.API.Configurations.Endpoint;
using BuildingBlocks.API.Extensions;
using MediatR;
using Shared.Models.Exceptions;

namespace API.Features.Login;

public class LoginEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/dummy/login", async (IServiceProvider serviceProvider, ISender mediator, CancellationToken ct) =>
            {
                var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                if (environment.IsProduction()) throw new DomainException("currently in production environment");

                var command = new LoginCommand();
                var response = await mediator.Send(command, ct);
                return response.ToResult();
            })
            .WithName(nameof(LoginEndpoint))
            .WithTags("Dummy Endpoints");
    }
}