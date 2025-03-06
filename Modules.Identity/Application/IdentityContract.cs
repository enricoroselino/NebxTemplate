using Modules.Identity.Contract;
using Modules.Identity.Contract.Dtos;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Models;
using Modules.SharedKernel;

namespace Modules.Identity.Application;

public class IdentityContract : IIdentityContract
{
    private readonly IUserRepository _userRepository;
    private readonly AppIdentityDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public IdentityContract(
        IUserRepository userRepository,
        AppIdentityDbContext dbContext,
        UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<bool> IsUserExists(
        Guid? userId = null,
        int? compatId = null,
        string? identifier = null,
        CancellationToken ct = default)
    {
        var user = await _userRepository
            .GetUser(userId: userId, compatId: compatId, identifier: identifier, tracking: false, ct: ct);
        return user is not null;
    }

    public async Task<UserDto?> GetUser(
        Guid? userId = null,
        int? compatId = null,
        string? identifier = null,
        CancellationToken ct = default)
    {
        var user = await _userRepository
            .GetUser(userId: userId, compatId: compatId, identifier: identifier, tracking: false, ct: ct);
        return user is null ? null : new UserDto(user.Id, user.CompatId, user.FullName, user.Email);
    }

    public async Task<Verdict<UserId>> MigrateUser(UserMigrateDto dto, CancellationToken ct = default)
    {
        var newUser = User.Migrate(dto.Username, dto.Email, dto.FullNme, dto.CompatId);

        var existing = await _dbContext.Users
            .AsNoTracking()
            .Where(x =>
                x.NormalizedUserName == newUser.UserName.ToUpperInvariant() ||
                x.NormalizedEmail == newUser.Email.ToUpperInvariant())
            .SingleOrDefaultAsync(ct);

        if (existing is not null) return Verdict.Conflict("User already exists.");
        
        var result = await _userManager.CreateAsync(newUser, dto.Password);
        return result.Succeeded ? Verdict.Success(new UserId(newUser.Id)) : Verdict.InternalError(result.GetErrors());
    }
}