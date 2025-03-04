using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Services;

namespace Modules.Identity.Features.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Verdict<Response<RefreshTokenResponse>>>
{
    private readonly ITokenServices _tokenServices;
    private readonly IUserRepository _userRepository;

    public RefreshTokenCommandHandler(ITokenServices tokenServices, IUserRepository userRepository)
    {
        _tokenServices = tokenServices;
        _userRepository = userRepository;
    }

    public async Task<Verdict<Response<RefreshTokenResponse>>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(userId: request.UserId, tracking: false, ct: cancellationToken);
        if (user is null) return Verdict.Unauthorized();

        var claims = _userRepository.GetInformationClaims(user);
        var refreshResult = await _tokenServices.RefreshToken(
            user,
            request.TokenId,
            request.RefreshToken,
            claims,
            cancellationToken);

        if (!refreshResult.IsSuccess) return Verdict.InternalError(refreshResult.ErrorMessage);

        var responseDto = new RefreshTokenResponse(refreshResult.Value.AccessToken, refreshResult.Value.RefreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}