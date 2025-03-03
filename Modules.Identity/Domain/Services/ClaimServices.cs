using System.Security.Claims;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;

namespace Modules.Identity.Domain.Services;

public interface IClaimServices
{
    public Task<List<Claim>> GetClaims(Guid userId, CancellationToken ct = default);
}

public class ClaimServices : IClaimServices
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly AppIdentityDbContext _dbContext;

    public ClaimServices(IUserRepository userRepository, IRoleRepository roleRepository, AppIdentityDbContext dbContext)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _dbContext = dbContext;
    }

    public async Task<List<Claim>> GetClaims(Guid userId, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FindAsync([userId], ct);
        if (user is null) return [];

        var userClaims = await _userRepository.GetUserClaims(user);
        var roleClaims = await _roleRepository.GetRoleClaims(user, ct);
        var roles = await _roleRepository.GetRoles(user, ct);

        return [..userClaims, ..roleClaims, ..roles];
    }
}