using BuildingBlocks.API.Configurations.Endpoint;
using Microsoft.AspNetCore.Mvc;
using Modules.Identity.Constants;

namespace Modules.Identity.Features.Authentication.ChangePassword;

public record ChangePasswordRequest(string OldPassword, string Password, string PasswordConfirmation);

public class ChangePasswordEndpoint : IEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ApiMeta.Authentication.Route);

        group.MapPost("change-password", async (
                ISender mediator,
                HttpContext httpContext,
                ClaimsPrincipal principal,
                CancellationToken ct,
                [FromBody] ChangePasswordRequest dto) =>
            {
                var userId = principal.GetUserId();
                var tokenId = principal.GetTokenId();
                if (userId is null || tokenId is null)
                {
                    return Verdict.Unauthorized().ToResult(httpContext);
                }
                
                var command = new ChangePasswordCommand(
                    userId.Value,
                    dto.OldPassword,
                    dto.Password,
                    dto.PasswordConfirmation,
                    tokenId.Value);

                var result = await mediator.Send(command, ct);
                return result.ToResult(httpContext);
            })
            .WithName(nameof(ChangePasswordEndpoint))
            .WithSummary("change user password")
            .WithTags(ApiMeta.Authentication.Tag)
            .RequireAuthorization();
    }
}