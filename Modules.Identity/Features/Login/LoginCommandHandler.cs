using BuildingBlocks.API.Models.CQRS;
using Modules.Identity.Domain.Services;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Verdict<Response<LoginResponse>>>
{
    private readonly ILoginServices _loginServices;

    public LoginCommandHandler(ILoginServices loginServices)
    {
        _loginServices = loginServices;
    }

    public async Task<Verdict<Response<LoginResponse>>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var tokenResult = await _loginServices.Authenticate(request.Identifier, request.Password, ct: cancellationToken);
        if (!tokenResult.IsSuccess) return Verdict.Unauthorized(tokenResult.ErrorMessage);

        var responseDto = new LoginResponse(tokenResult.Value.AccessToken, tokenResult.Value.RefreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}