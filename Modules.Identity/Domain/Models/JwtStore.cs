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

    public virtual User User { get; init; } = null!;

    public static JwtStore Create(Guid userId, Guid tokenId, string refreshToken, long expiresOn)
    {
        var expire = DateTime.UtcNow.AddSeconds(expiresOn);

        return new JwtStore()
        {
            Id = Guid.CreateVersion7(),
            UserId = Guard.Against.NullOrEmpty(userId, nameof(userId),
                exceptionCreator: () => new DomainException("UserId cannot be null or empty.")),
            TokenId = Guard.Against.NullOrEmpty(tokenId, nameof(tokenId),
                exceptionCreator: () => new DomainException("TokenId cannot be null or empty.")),
            RefreshToken = Guard.Against.NullOrWhiteSpace(refreshToken, nameof(refreshToken),
                exceptionCreator: () => new DomainException("Refresh token cannot be empty.")),
            ExpiresOn = Guard.Against.NullOrOutOfSQLDateRange(expire, nameof(expiresOn),
                exceptionCreator: () => new DomainException("Expires time is not valid.")),
        };
    }

    public void Revoke() => RevokedOn = DateTime.UtcNow;

    public void Update(Guid tokenId, string refreshToken, long expiresOn)
    {
        var expire = DateTime.UtcNow.AddSeconds(expiresOn);

        TokenId = Guard.Against.NullOrEmpty(tokenId, nameof(tokenId),
            exceptionCreator: () => new DomainException("TokenId cannot be null or empty."));
        RefreshToken = Guard.Against.NullOrWhiteSpace(refreshToken, nameof(refreshToken),
            exceptionCreator: () => new DomainException("Refresh token cannot be null or empty."));
        ExpiresOn = Guard.Against.NullOrOutOfSQLDateRange(expire, nameof(expiresOn),
            exceptionCreator: () => new DomainException("Expires time is not valid."));
    }
}