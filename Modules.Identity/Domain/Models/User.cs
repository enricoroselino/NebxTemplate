using Ardalis.GuardClauses;
using BuildingBlocks.API.Models.DDD;
using Microsoft.AspNetCore.Identity;
using Shared.Models.Exceptions;
using Shared.Models.Interfaces;

namespace Modules.Identity.Domain.Models;

public class User : IdentityUser<Guid>, IEntity<Guid>
{
    private User()
    {
    }

    public int? CompatId { get; private set; }
    public override string UserName { get; set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public override string NormalizedUserName { get; set; } = string.Empty;
    public override string Email { get; set; } = string.Empty;
    public override string NormalizedEmail { get; set; } = string.Empty;
    public override string PasswordHash { get; set; } = string.Empty;
    public override string SecurityStamp { get; set; } = string.Empty;
    public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    
    public virtual ICollection<UserRole> UserRoles { get; private set; }
    public virtual ICollection<UserClaim> UserClaims { get; private set; }
    public virtual ICollection<UserLogin> UserLogins { get; private set; }
    public virtual ICollection<UserToken> UserTokens { get; private set; }

    public static User Create(string username, string email, string fullname, int? compatId = null)
    {
        return new User()
        {
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