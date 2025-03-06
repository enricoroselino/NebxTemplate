using BCrypt.Net;

namespace BuildingBlocks.API.Services;

public interface IHasher
{
    public string Hash(string input);
    public bool VerifyHash(string input, string hashed);
}

/// <summary>
/// OWASP work factor minimum recommendation is 10, but 14 is recommended.<br/>
/// </summary>
public class BcryptHasher : IHasher
{
    public string Hash(string input)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(input, 12, hashType: HashType.SHA512);
    }

    public bool VerifyHash(string input, string hashed)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(input, hashed, hashType: HashType.SHA512);
    }
}