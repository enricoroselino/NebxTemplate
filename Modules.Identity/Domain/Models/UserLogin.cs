﻿using Microsoft.AspNetCore.Identity;
using Shared.Models.Interfaces;

namespace Modules.Identity.Domain.Models;

public class UserLogin : IdentityUserLogin<Guid>, ITimeAuditable
{
    private UserLogin()
    {
    }

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}