using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Guid = System.Guid;

namespace BuildingBlocks.Contract.Infrastructure.JwtManager;

public sealed class JwtManager : IJwtManager
{
    private readonly JwtManagerOptions _options;
    private readonly TokenValidationParameters _validationParameters;
    private readonly TimeProvider _timeProvider;

    public JwtManager(
        IOptions<JwtManagerOptions> options,
        TokenValidationParameters validationParameters,
        TimeProvider timeProvider)
    {
        _options = options.Value;
        _validationParameters = validationParameters;
        _timeProvider = timeProvider;
    }

    private static JwtSecurityTokenHandler TokenHandler => new();
    private const string Algorithm = SecurityAlgorithms.HmacSha256;

    public TokenResult<Guid> CreateAccessToken(List<Claim> claims)
    {
        var audiences = _options.ValidAudiences
            .Select(x => new Claim(JwtRegisteredClaimNames.Aud, x));

        var tokenId = Guid.NewGuid();
        claims.AddRange(audiences);
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, tokenId.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Typ, JwtBearerDefaults.AuthenticationScheme));

        var keyBytes = Encoding.UTF8.GetBytes(_options.Key);
        var symmetricKey = new SymmetricSecurityKey(keyBytes);

        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var expiresIn = now.Add(_options.TokenSpan);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.ValidIssuer,
            SigningCredentials = new SigningCredentials(symmetricKey, Algorithm),
            Subject = new ClaimsIdentity(claims),
            Expires = expiresIn,
            IssuedAt = now,
            NotBefore = now
        };

        var tokenBase = TokenHandler.CreateToken(descriptor);
        var token = TokenHandler.WriteToken(tokenBase);
        var totalSeconds = (int)_options.TokenSpan.TotalSeconds;
        return TokenResult.Create(tokenId, token, totalSeconds);
    }

    public TokenResult CreateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(randomBytes);
        var totalSeconds = (int)_options.RefreshTokenSpan.TotalSeconds;
        return TokenResult.Create(refreshToken, totalSeconds);
    }

    public async Task<TokenValidationResult> Validate(string token)
    {
        return await TokenHandler.ValidateTokenAsync(token, _validationParameters);
    }

    public JwtSecurityToken Read(string token)
    {
        return TokenHandler.ReadJwtToken(token);
    }
}