using Microsoft.AspNetCore.Identity;
using Shared.Models.Interfaces;

namespace Modules.Identity.Domain.Models;

public class UserToken : IdentityUserToken<Guid>, ITimeAuditable
{
    private UserToken()
    {
    }

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}