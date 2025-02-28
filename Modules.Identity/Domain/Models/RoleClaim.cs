using Microsoft.AspNetCore.Identity;
using Shared.Models.Interfaces;

namespace Modules.Identity.Domain.Models;

public class RoleClaim : IdentityRoleClaim<Guid>, ITimeAuditable
{
    private RoleClaim()
    {
    }

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}