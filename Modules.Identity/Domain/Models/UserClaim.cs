﻿using Microsoft.AspNetCore.Identity;
using Shared.Models.Interfaces;

namespace Modules.Identity.Domain.Models;

public class UserClaim : IdentityUserClaim<Guid>, ITimeAuditable
{
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual User User { get; set; } = null!;
}