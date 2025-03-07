﻿using Shared.Models.Interfaces;

namespace Modules.Identity.Domain.Models;

public class UserLogin : IdentityUserLogin<Guid>, ITimeAuditable
{
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual User User { get; init; } = null!;
}