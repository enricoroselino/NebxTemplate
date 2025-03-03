using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Repository;

public interface IUserRepository
{
    public List<Claim> GetInformationClaims(User user);
}

public class UserRepository : IUserRepository
{
    public List<Claim> GetInformationClaims(User user)
    {
        var informationClaims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.PreferredUsername, user.UserName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.EmailVerified, user.EmailConfirmed ? "1" : "0",
                ClaimValueTypes.Integer32),
        };

        return informationClaims;
    }
}