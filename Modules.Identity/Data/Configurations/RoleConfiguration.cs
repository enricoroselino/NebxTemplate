﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id)
            .IsClustered(false);
        
        builder.HasIndex(x => x.CreatedOn)
            .IsClustered();

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(150)
            .IsRequired();
    }
}