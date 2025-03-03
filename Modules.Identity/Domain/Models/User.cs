using Ardalis.GuardClauses;
using BuildingBlocks.API.Models.DDD;
using Microsoft.AspNetCore.Identity;
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
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public virtual ICollection<UserClaim> UserClaims { get; private set; } = new List<UserClaim>();
    public virtual ICollection<UserLogin> UserLogins { get; private set; } = new List<UserLogin>();
    public virtual ICollection<UserToken> UserTokens { get; private set; } = new List<UserToken>();

    public static User Create(string username, string email, string fullname, int? compatId = null)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            UserName = Guard.Against.NullOrWhiteSpace(username,
                exceptionCreator: () => new DomainException("Username can't be empty.")),
            Email = Guard.Against.NullOrWhiteSpace(email,
                exceptionCreator: () => new DomainException("Email can't be empty.")),
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = email.ToUpperInvariant(),
            FullName = Guard.Against.NullOrWhiteSpace(fullname,
                exceptionCreator: () => new DomainException("Fullname can't be empty.")),
            CompatId = compatId
        };
    }
}