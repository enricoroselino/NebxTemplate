using Microsoft.AspNetCore.Identity;

namespace Modules.Identity.Domain.Models;

public class UserRole : IdentityUserRole<Guid>
{
    private UserRole()
    {
    }
    
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
}