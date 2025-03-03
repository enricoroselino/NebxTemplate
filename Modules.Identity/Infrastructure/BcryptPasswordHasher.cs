using BuildingBlocks.API.Services;
using Microsoft.AspNetCore.Identity;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Infrastructure;

public class BcryptPasswordHasher : IPasswordHasher<User>
{
    private readonly IHasher _hasher;

    public BcryptPasswordHasher(IHasher hasher)
    {
        _hasher = hasher;
    }

    public string HashPassword(User user, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
        return _hasher.Hash(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword, nameof(hashedPassword));
        ArgumentException.ThrowIfNullOrWhiteSpace(providedPassword, nameof(providedPassword));

        try
        {
            return _hasher.VerifyHash(providedPassword, hashedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
        catch
        {
            return PasswordVerificationResult.Failed;
        }
    }
}