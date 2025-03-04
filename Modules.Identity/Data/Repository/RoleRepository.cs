using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Repository;

public interface IRoleRepository
{
    public Task<List<Claim>> GetRoles(User user, CancellationToken ct = default);
    public Task<List<Claim>> GetRoleClaims(User user, CancellationToken ct = default);
}

public class RoleRepository : IRoleRepository
{
    private readonly AppIdentityDbContext _dbContext;

    public RoleRepository(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Claim>> GetRoles(User user, CancellationToken ct = default)
    {
        var roles = await _dbContext.UserRoles
            .AsNoTracking()
            .Include(x => x.Role)
            .Where(x => x.UserId == user.Id)
            .Select(x => new Claim(ClaimTypes.Role, x.Role.Name))
            .ToListAsync(ct);

        return roles;
    }

    public async Task<List<Claim>> GetRoleClaims(User user, CancellationToken ct = default)
    {
        var roleClaims = await _dbContext.RoleClaims
            .AsNoTracking()
            .Where(x => x.Role.UserRoles.Any(y => y.UserId == user.Id))
            .Select(x => new Claim(x.ClaimType, x.ClaimValue))
            .ToListAsync(ct);

        return roleClaims;
    }
}