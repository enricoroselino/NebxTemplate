using BuildingBlocks.API.Services.JwtManager;
using Modules.Identity.Data;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Domain.Services;

public interface ITokenServices
{
    public Task<Verdict> RevokeToken(User user, Guid tokenId, CancellationToken ct = default);
    public Task<Verdict<TokenResultPair>> IssueToken(User user, List<Claim> claims, CancellationToken ct = default);

    public Task<Verdict<TokenResultPair>> RefreshToken(
        User user,
        Guid tokenId,
        string refreshToken,
        List<Claim> claims, 
        CancellationToken ct = default);
}

public class TokenServices : ITokenServices
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IJwtManager _jwtManager;

    public TokenServices(AppIdentityDbContext dbContext, IJwtManager jwtManager)
    {
        _dbContext = dbContext;
        _jwtManager = jwtManager;
    }

    public async Task<Verdict> RevokeToken(
        User user,
        Guid tokenId,
        CancellationToken ct = default)
    {
        var tokenData = await _dbContext.JwtStores
            .SingleOrDefaultAsync(x => x.UserId == user.Id && x.TokenId == tokenId, ct);
        if (tokenData is null) return Verdict.InternalError("Token data not found");

        tokenData.Revoke();
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

        var tokenData = JwtStore.Create(user.Id, accessToken.Id, refreshToken.Value, refreshToken.ExpiresOn);

        var existing = await _dbContext.JwtStores
            .AnyAsync(x => x.UserId == tokenData.UserId && x.TokenId == tokenData.Id, ct);
        if (existing) return Verdict.InternalError("Token already exists");
        
        await _dbContext.JwtStores.AddAsync(tokenData, ct);
        await _dbContext.SaveChangesAsync(ct);

        var tokenPairDto = new TokenResultPair(accessToken, refreshToken);
        return Verdict.Success(tokenPairDto);
    }

    public async Task<Verdict<TokenResultPair>> RefreshToken(
        User user,
        Guid tokenId,
        string refreshToken,
        List<Claim> claims,
        CancellationToken ct = default)
    {
        var tokenData = await _dbContext.JwtStores
            .Where(x => x.UserId == user.Id && x.TokenId == tokenId)
            .Where(x => x.RevokedOn == null && x.ExpiresOn > DateTime.UtcNow)
            .SingleOrDefaultAsync(ct);

        if (tokenData is null) return Verdict.InternalError("Token data not found");
        if (!tokenData.RefreshToken.Equals(refreshToken)) return Verdict.InternalError("Refresh token not match");

        var newAccessToken = _jwtManager.CreateAccessToken(claims);
        var newRefreshToken = _jwtManager.CreateRefreshToken();
        
        tokenData.Update(newAccessToken.Id, newRefreshToken.Value, newRefreshToken.ExpiresOn);
        await _dbContext.SaveChangesAsync(ct);
        
        var tokenPairDto = new TokenResultPair(newAccessToken, newRefreshToken);
        return Verdict.Success(tokenPairDto);
    }
}