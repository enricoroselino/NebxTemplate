using Ardalis.GuardClauses;
using BuildingBlocks.API.Models.DDD;
using Shared.Models.Exceptions;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace Modules.Identity.Domain.Models;

public class Role : IdentityRole<Guid>, IEntity<Guid>
{
    public override string Name { get; set; } = string.Empty;
    public override string NormalizedName { get; set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<RoleClaim> RoleClaims { get; init; } = new List<RoleClaim>();
    public virtual ICollection<UserRole> UserRoles { get; init; } = new List<UserRole>();

    public static Role Create(string name, string description)
    {
        return new Role()
        {
            Id = Guid.CreateVersion7(),
            Name = Guard.Against.NullOrWhiteSpace(name,
                exceptionCreator: () => new DomainException("Name can't be empty.")),
            NormalizedName = name.ToUpperInvariant(),
            Description = Guard.Against.NullOrWhiteSpace(description,
                exceptionCreator: () => new DomainException("Please give meaningful description.")),
        };
    }
}