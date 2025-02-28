﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(x => x.Id)
            .IsClustered(false);

        builder.HasIndex(x => x.CreatedOn)
            .IsClustered();

        builder.Property(x => x.Description)
            .HasMaxLength(150)
            .IsRequired();
    }
}