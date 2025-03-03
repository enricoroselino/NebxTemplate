using System.Security.Claims;
using BuildingBlocks.API.Models.CQRS;
using BuildingBlocks.API.Services.JwtManager;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Shared;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.Impersonate;

public class ImpersonateCommandHandler : ICommandHandler<ImpersonateCommand, Verdict<Response<ImpersonateResponse>>>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;

    public ImpersonateCommandHandler(
        AppIdentityDbContext dbContext, 
        IUserRepository userRepository, 
        IJwtManager jwtManager)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _jwtManager = jwtManager;
    }

    public async Task<Verdict<Response<ImpersonateResponse>>> Handle(ImpersonateCommand request, CancellationToken cancellationToken)
    {
        var targetUser = await _dbContext.Users.FindAsync([request.UserId], cancellationToken);
        if (targetUser is null) return Verdict.NotFound("User not found");

        var claims = _userRepository.GetInformationClaims(targetUser);
        claims.Add(new Claim(CustomClaim.ImpersonatorId, request.CurrentUserId.ToString()));
        
        var accessToken = _jwtManager.CreateAccessToken(claims);
        var refreshToken = _jwtManager.CreateRefreshToken();
        
        var responseDto = new ImpersonateResponse(accessToken, refreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}