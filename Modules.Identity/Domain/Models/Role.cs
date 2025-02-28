using Ardalis.GuardClauses;
using BuildingBlocks.API.Models.DDD;
using Microsoft.AspNetCore.Identity;
using Shared.Models.Exceptions;

namespace Modules.Identity.Domain.Models;

public class Role : IdentityRole<Guid>, IEntity<Guid>
{
    private Role()
    {
    }

    public override string Name { get; set; } = string.Empty;
    public override string NormalizedName { get; set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<RoleClaim> RoleClaims { get; private set; }
    public virtual ICollection<UserRole> UserRoles { get; private set; }

    public static Role Create(string name, string description)
    {
        return new Role()
        {
            Id = Guid.CreateVersion7(),
            Name = Guard.Against.NullOrWhiteSpace(name,
                exceptionCreator: () => new DomainException("Name can't be empty.")),
            NormalizedName = name.ToUpperInvariant(),
            Description = description,
        };
    }
}