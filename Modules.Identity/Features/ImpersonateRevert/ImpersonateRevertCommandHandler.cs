using BuildingBlocks.API.Models.CQRS;
using BuildingBlocks.API.Services.JwtManager;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Models;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.ImpersonateRevert;

public class ImpersonateRevertCommandHandler
    : ICommandHandler<ImpersonateRevertCommand, Verdict<Response<ImpersonateRevertResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;
    private readonly TimeProvider _timeProvider;
    private readonly AppIdentityDbContext _dbContext;

    public ImpersonateRevertCommandHandler(
        IUserRepository userRepository,
        IJwtManager jwtManager,
        AppIdentityDbContext dbContext,
        TimeProvider timeProvider)
    {
        _userRepository = userRepository;
        _jwtManager = jwtManager;
        _dbContext = dbContext;
        _timeProvider = timeProvider;
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

        var revertDate = _timeProvider.GetUtcNow().DateTime;
        var refreshTokenExpiresOn = revertDate.AddSeconds(refreshToken.ExpiresOn);

        var tokenData = JwtStore.Create(user.Id, accessToken.Id, refreshToken.Value, refreshTokenExpiresOn);
        await _dbContext.JwtStores.AddAsync(tokenData, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var responseDto = new ImpersonateRevertResponse(accessToken, refreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}