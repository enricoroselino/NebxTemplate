using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasIndex(x => x.Id)
            .IsClustered(false);
        
        builder.HasIndex(x => x.CreatedOn)
            .IsClustered();

        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.FullName)
            .HasMaxLength(150)
            .IsRequired();
    }
}