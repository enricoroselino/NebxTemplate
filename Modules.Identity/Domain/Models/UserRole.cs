namespace Modules.Identity.Domain.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; init; } = null!;
    public virtual Role Role { get; init; } = null!;

    public static UserRole Create(User user, Role role)
    {
        return new UserRole()
        {
            UserId = user.Id,
            RoleId = role.Id,
        };
    }
}