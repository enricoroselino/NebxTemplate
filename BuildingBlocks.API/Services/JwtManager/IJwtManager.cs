using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Contract.Infrastructure.JwtManager;

public interface IJwtManager
{
    public TokenResult<Guid> CreateAccessToken(List<Claim> claims);
    public TokenResult CreateRefreshToken();
    public Task<TokenValidationResult> Validate(string token);
    public JwtSecurityToken Read(string token);
}