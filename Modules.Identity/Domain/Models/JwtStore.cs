using Ardalis.GuardClauses;
using BuildingBlocks.API.Models.DDD;
using Shared.Models.Exceptions;

namespace Modules.Identity.Domain.Models;

public class JwtStore : Entity<Guid>
{
    private JwtStore()
    {
    }

    public Guid UserId { get; private set; }
    public Guid TokenId { get; private set; }
    public string RefreshToken { get; private set; } = string.Empty;
    public DateTime ExpiresOn { get; private set; }
    public DateTime? RevokedOn { get; private set; }

    public static JwtStore Create(Guid userId, Guid tokenId, string refreshToken, DateTime expiresOn)
    {
        return new JwtStore()
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            TokenId = tokenId,
            RefreshToken = Guard.Against.NullOrWhiteSpace(refreshToken, nameof(refreshToken),
                exceptionCreator: () => new DomainException("Refresh token cannot be empty.")),
            ExpiresOn = Guard.Against.NullOrOutOfSQLDateRange(expiresOn, nameof(expiresOn),
                exceptionCreator: () => new DomainException("Expires time is not valid.")),
        };
    }

    public void Revoke(DateTime revokedOn) => RevokedOn = Guard.Against
        .NullOrOutOfSQLDateRange(revokedOn, nameof(revokedOn),
            exceptionCreator: () => new DomainException("Revoked time is not valid."));

    public void UnRevoke() => RevokedOn = null;

    public void Update(Guid tokenId, string refreshToken, DateTime expiresOn)
    {
        TokenId = tokenId;
        RefreshToken = refreshToken;
        ExpiresOn = expiresOn;
    }
}