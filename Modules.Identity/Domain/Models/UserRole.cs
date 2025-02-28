using Microsoft.AspNetCore.Identity;

namespace Modules.Identity.Domain.Models;

public class UserRole : IdentityUserRole<Guid>
{
    private UserRole()
    {
    }
}