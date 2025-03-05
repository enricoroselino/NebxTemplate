using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Services;

namespace Modules.Identity.Features.Authentication.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Verdict<Response<LoginResponse>>>
{
    private readonly IAuthServices _authServices;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(IAuthServices authServices, IUserRepository userRepository)
    {
        _authServices = authServices;
        _userRepository = userRepository;
    }

    public async Task<Verdict<Response<LoginResponse>>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(identifier: request.Identifier, tracking: true, ct: cancellationToken);
        if (user is null) return Verdict.Unauthorized("Username or password is incorrect");

        var loginResult = await _authServices.Login(user, request.Password, ct: cancellationToken);
        if (!loginResult.IsSuccess) return Verdict.Unauthorized(loginResult.ErrorMessage);

        var responseDto = new LoginResponse(loginResult.Value.AccessToken, loginResult.Value.RefreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}