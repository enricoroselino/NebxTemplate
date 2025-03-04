using Modules.Identity.Domain.Services;

namespace Modules.Identity.Features.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Verdict<Response<LoginResponse>>>
{
    private readonly IAuthServices _authServices;

    public LoginCommandHandler(IAuthServices authServices)
    {
        _authServices = authServices;
    }

    public async Task<Verdict<Response<LoginResponse>>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var loginResult = await _authServices.Login(request.Identifier, request.Password, ct: cancellationToken);
        if (!loginResult.IsSuccess) return Verdict.Unauthorized(loginResult.ErrorMessage);

        var responseDto = new LoginResponse(loginResult.Value.AccessToken, loginResult.Value.RefreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}