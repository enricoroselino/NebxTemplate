using System.Security.Claims;
using BuildingBlocks.API.Models.CQRS;
using BuildingBlocks.API.Services.JwtManager;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Shared;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.Impersonate;

public class ImpersonateCommandHandler : ICommandHandler<ImpersonateCommand, Verdict<Response<ImpersonateResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;
    private readonly AppIdentityDbContext _dbContext;
    private readonly TimeProvider _timeProvider;

    public ImpersonateCommandHandler(
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

    public async Task<Verdict<Response<ImpersonateResponse>>> Handle(ImpersonateCommand request,
        CancellationToken cancellationToken)
    {
        var targetUser = await _userRepository.GetUser(userId: request.TargetUserId, tracking: false, ct: cancellationToken);
        if (targetUser is null) return Verdict.NotFound("User not found");

        var claims = _userRepository.GetInformationClaims(targetUser);
        claims.Add(new Claim(CustomClaim.ImpersonatorId, request.CurrentUserId.ToString()));

        var accessToken = _jwtManager.CreateAccessToken(claims);
        var refreshToken = _jwtManager.CreateRefreshToken();

        var tokenData = await _dbContext.JwtStores
            .SingleOrDefaultAsync(x =>
                x.UserId == request.CurrentUserId &&
                x.TokenId == request.TokenId, cancellationToken);

        if (tokenData is null) return Verdict.NotFound("Token data not found");

        var revokedOn = _timeProvider.GetUtcNow().DateTime;
        tokenData.Revoke(revokedOn);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var responseDto = new ImpersonateResponse(accessToken, refreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}