using BuildingBlocks.API.Models.CQRS;
using BuildingBlocks.API.Services.JwtManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Models;
using Shared.Models.Responses;
using Shared.Verdict;

namespace Modules.Identity.Features.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, Verdict<Response<LoginResponse>>>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;
    private readonly TimeProvider _time;

    public LoginCommandHandler(
        AppIdentityDbContext dbContext,
        SignInManager<User> signInManager,
        IUserRepository userRepository,
        IJwtManager jwtManager,
        TimeProvider time)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _jwtManager = jwtManager;
        _time = time;
    }

    public async Task<Verdict<Response<LoginResponse>>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var loginDate = _time.GetUtcNow().DateTime;
        
        var user = await _dbContext.Users
            .Where(x => x.UserName == request.Identifier || x.Email == request.Identifier)
            .Where(x => x.IsActive)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null) return Verdict.Unauthorized("Username or password is incorrect");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded) return Verdict.Unauthorized("Username or password is incorrect");

        user.Login(loginDate);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var claims = _userRepository.GetInformationClaims(user);
        var accessToken = _jwtManager.CreateAccessToken(claims);
        var refreshToken = _jwtManager.CreateRefreshToken();

        var responseDto = new LoginResponse(accessToken, refreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}