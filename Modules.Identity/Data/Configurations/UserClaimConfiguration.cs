﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany(x => x.UserClaims)
            .HasForeignKey(x => x.UserId);
    }
}