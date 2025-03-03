using Microsoft.AspNetCore.Identity;

namespace Modules.Identity.Domain.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; init; } = null!;
    public virtual Role Role { get; init; } = null!;
}