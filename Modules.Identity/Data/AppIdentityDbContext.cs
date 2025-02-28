using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data;

public class AppIdentityDbContext
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    private readonly IConfiguration _configuration;

    public AppIdentityDbContext(
        DbContextOptions<AppIdentityDbContext> options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    private const string Schema = "Identity";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var conn = _configuration.GetConnectionString("AppDb");
        ArgumentException.ThrowIfNullOrEmpty(conn, nameof(conn));

        optionsBuilder.UseSqlServer(conn, builder => builder.MigrationsHistoryTable("__EFMigrationsHistory", Schema));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppIdentityDbContext).Assembly);

        modelBuilder.Entity<User>(entity => entity.ToTable("Users"));
        modelBuilder.Entity<Role>(entity => entity.ToTable("Roles"));
        modelBuilder.Entity<UserRole>(entity => entity.ToTable("UserRoles"));
        modelBuilder.Entity<UserClaim>(entity => entity.ToTable("UserClaims"));
        modelBuilder.Entity<RoleClaim>(entity => entity.ToTable("RoleClaims"));
        modelBuilder.Entity<UserLogin>(entity => entity.ToTable("UserLogins"));
        modelBuilder.Entity<UserToken>(entity => entity.ToTable("UserTokens"));
    }
}