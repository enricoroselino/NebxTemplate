using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Configurations;

public class JwtStoreConfiguration : IEntityTypeConfiguration<JwtStore>
{
    public void Configure(EntityTypeBuilder<JwtStore> builder)
    {
        builder.ToTable("JwtStores");

        builder.HasKey(x => x.Id)
            .IsClustered(false);

        builder.HasIndex(x => new { x.UserId, x.TokenId })
            .IsUnique()
            .IsClustered();

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.TokenId)
            .IsRequired();

        builder.Property(x => x.RefreshToken)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.ExpiresOn)
            .IsRequired();
    }
}