using Modules.Identity.Contract;
using Modules.Identity.Contract.Dtos;
using Modules.Identity.Data.Repository;

namespace Modules.Identity.Application;

public class IdentityContract : IIdentityContract
{
    private readonly IUserRepository _userRepository;

    public IdentityContract(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
        return user is null
            ? null
            : new UserDto(user.Id, user.CompatId, user.FullName, user.Email);
    }
}