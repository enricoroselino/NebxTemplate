using Shared.Models.Interfaces;
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

namespace Modules.Identity.Domain.Models;

public class RoleClaim : IdentityRoleClaim<Guid>, ITimeAuditable
{
    public override string ClaimType { get; set; } = string.Empty;
    public override string ClaimValue { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual Role Role { get; init; } = null!;
}