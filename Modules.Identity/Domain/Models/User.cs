using Ardalis.GuardClauses;
using BuildingBlocks.API.Models.DDD;
using Shared.Models.Exceptions;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace Modules.Identity.Domain.Models;

public class User : IdentityUser<Guid>, IEntity<Guid>
{
    public override string UserName { get; set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public override string NormalizedUserName { get; set; } = string.Empty;
    public override string Email { get; set; } = string.Empty;
    public override string NormalizedEmail { get; set; } = string.Empty;
    public override string PasswordHash { get; set; } = string.Empty;
    public int? CompatId { get; private set; }
    public DateTime? LastLogin { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public virtual ICollection<UserClaim> UserClaims { get; private set; } = new List<UserClaim>();
    public virtual ICollection<UserLogin> UserLogins { get; private set; } = new List<UserLogin>();
    public virtual ICollection<UserToken> UserTokens { get; private set; } = new List<UserToken>();
    public virtual ICollection<JwtStore> JwtStores { get; private set; } = new List<JwtStore>();

    public static User Create(string username, string email, string fullname, int? compatId = null)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            CompatId = compatId,
            UserName = Guard.Against.NullOrWhiteSpace(username,
                exceptionCreator: () => new DomainException("Username can't be empty.")),
            NormalizedUserName = username.ToUpperInvariant(),
            Email = Guard.Against.NullOrWhiteSpace(email,
                exceptionCreator: () => new DomainException("Email can't be empty.")),
            NormalizedEmail = email.ToUpperInvariant(),
            FullName = Guard.Against.NullOrWhiteSpace(fullname,
                exceptionCreator: () => new DomainException("Fullname can't be empty.")),
            IsActive = true,
        };
    }

    public void Login() => LastLogin = DateTime.UtcNow;
    public void Deactivate() => IsActive = false;

    public static User Migrate(string username, string email, string fullname, int compatId)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            CompatId = Guard.Against.NegativeOrZero(compatId, nameof(compatId),
                exceptionCreator: () => new DomainException("CompatId is not valid.")),
            UserName = Guard.Against.NullOrWhiteSpace(username,
                exceptionCreator: () => new DomainException("Username can't be empty.")),
            NormalizedUserName = username.ToUpperInvariant(),
            Email = Guard.Against.NullOrWhiteSpace(email,
                exceptionCreator: () => new DomainException("Email can't be empty.")),
            NormalizedEmail = email.ToUpperInvariant(),
            FullName = Guard.Against.NullOrWhiteSpace(fullname,
                exceptionCreator: () => new DomainException("Fullname can't be empty.")),
            IsActive = true,
        };
    }
}