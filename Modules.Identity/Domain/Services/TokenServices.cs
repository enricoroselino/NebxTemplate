using System.Security.Claims;
using BuildingBlocks.API.Services.JwtManager;
using Microsoft.EntityFrameworkCore;
using Modules.Identity.Data;
using Modules.Identity.Domain.Models;
using Shared.Verdict;

namespace Modules.Identity.Domain.Services;

public interface ITokenServices
{
    public Task<Verdict> RevokeToken(Guid userId, Guid tokenId, CancellationToken ct = default);
    public Task<Verdict<TokenResultPair>> IssueToken(User user, List<Claim> claims, CancellationToken ct = default);
}

public class TokenServices : ITokenServices
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly TimeProvider _timeProvider;
    private readonly IJwtManager _jwtManager;

    public TokenServices(AppIdentityDbContext dbContext, TimeProvider timeProvider, IJwtManager jwtManager)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
        _jwtManager = jwtManager;
    }

    public async Task<Verdict> RevokeToken(
        Guid userId,
        Guid tokenId,
        CancellationToken ct = default)
    {
        var tokenData = await _dbContext.JwtStores
            .SingleOrDefaultAsync(x => x.UserId == userId && x.TokenId == tokenId, ct);
        if (tokenData is null) return Verdict.InternalError("Token data not found");

        var revokedOn = _timeProvider.GetUtcNow().DateTime;
        tokenData.Revoke(revokedOn);
        
        await _dbContext.SaveChangesAsync(ct);
        return Verdict.Success();
    }

    public async Task<Verdict<TokenResultPair>> IssueToken(
        User user,
        List<Claim> claims,
        CancellationToken ct = default)
    {
        var accessToken = _jwtManager.CreateAccessToken(claims);
        var refreshToken = _jwtManager.CreateRefreshToken();

        var issueDate = _timeProvider.GetUtcNow().DateTime;
        var expiresOn = issueDate.AddSeconds(refreshToken.ExpiresOn);
        var tokenData = JwtStore.Create(user.Id, accessToken.Id, refreshToken.Value, expiresOn);

        await _dbContext.JwtStores.AddAsync(tokenData, ct);
        await _dbContext.SaveChangesAsync(ct);

        var tokenPairDto = new TokenResultPair(accessToken, refreshToken);
        return Verdict.Success(tokenPairDto);
    }
}