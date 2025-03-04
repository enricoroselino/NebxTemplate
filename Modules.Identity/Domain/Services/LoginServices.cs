using System.Security.Claims;
using BuildingBlocks.API.Services.JwtManager;
using Microsoft.AspNetCore.Identity;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Models;
using Shared.Verdict;

namespace Modules.Identity.Domain.Services;

public interface ILoginServices
{
    public Task<Verdict<TokenResultPair>> Authenticate(
        string identifier, 
        string password, 
        List<Claim>? addOnClaims = null,
        CancellationToken ct = default);
}

public class LoginServices : ILoginServices
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;
    private readonly TimeProvider _timeProvider;

    public LoginServices(
        AppIdentityDbContext dbContext,
        SignInManager<User> signInManager,
        IUserRepository userRepository,
        IJwtManager jwtManager,
        TimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _jwtManager = jwtManager;
        _timeProvider = timeProvider;
    }

    public async Task<Verdict<TokenResultPair>> Authenticate(string identifier,
        string password,
        List<Claim>? addOnClaims = null,
        CancellationToken ct = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

        var user = await _userRepository.GetUser(identifier: identifier, tracking: true, ct: ct);
        if (user is null) return Verdict.Unauthorized("Username or password is incorrect");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded) return Verdict.Unauthorized("Username or password is incorrect");

        var claims = _userRepository.GetInformationClaims(user);
        var accessToken = _jwtManager.CreateAccessToken(claims.Concat(addOnClaims ?? []).ToList());
        var refreshToken = _jwtManager.CreateRefreshToken();

        var loginDate = _timeProvider.GetUtcNow().DateTime;
        var refreshTokenExpiresOn = loginDate.AddSeconds(refreshToken.ExpiresOn);
        var tokenData = JwtStore.Create(user.Id, accessToken.Id, refreshToken.Value, refreshTokenExpiresOn);

        user.Login(loginDate);
        await _dbContext.JwtStores.AddAsync(tokenData, ct);

        await _dbContext.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        var resultDto = new TokenResultPair(accessToken, refreshToken);
        return Verdict.Success(resultDto);
    }
}