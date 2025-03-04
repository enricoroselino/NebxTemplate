using BuildingBlocks.API.Models.CQRS;
using BuildingBlocks.API.Services.JwtManager;
using Modules.Identity.Data.Repository;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.ImpersonateRevert;

public class ImpersonateRevertCommandHandler
    : ICommandHandler<ImpersonateRevertCommand, Verdict<Response<ImpersonateRevertResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;

    public ImpersonateRevertCommandHandler(IUserRepository userRepository, IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _jwtManager = jwtManager;
    }

    public async Task<Verdict<Response<ImpersonateRevertResponse>>> Handle(
        ImpersonateRevertCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(userId: request.ImpersonatorId, tracking: false, ct: cancellationToken);
        if (user is null) return Verdict.NotFound("User not found");

        var claims = _userRepository.GetInformationClaims(user);
        var accessToken = _jwtManager.CreateAccessToken(claims);
        var refreshToken = _jwtManager.CreateRefreshToken();

        var responseDto = new ImpersonateRevertResponse(accessToken, refreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}