using System.Security.Claims;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Modules.Identity.Data.Specifications;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Repository;

public interface IUserRepository
{
    public List<Claim> GetInformationClaims(User user);
    public Task<List<Claim>> GetUserClaims(User user);

    public Task<User?> GetUser(
        Guid? userId = null,
        int? compatId = null,
        string? identifier = null,
        bool tracking = true,
        CancellationToken ct = default);
}

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly AppIdentityDbContext _dbContext;

    public UserRepository(UserManager<User> userManager, AppIdentityDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public List<Claim> GetInformationClaims(User user)
    {
        var informationClaims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.PreferredUsername, user.UserName),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.EmailVerified,
                user.EmailConfirmed ? "1" : "0", ClaimValueTypes.Integer32),
        };

        if (user.PhoneNumber is null) return informationClaims;

        informationClaims.Add(new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber));
        informationClaims.Add(new Claim(JwtRegisteredClaimNames.PhoneNumberVerified,
            user.PhoneNumberConfirmed ? "1" : "0", ClaimValueTypes.Integer32));
        return informationClaims;
    }

    public async Task<List<Claim>> GetUserClaims(User user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        return claims.ToList();
    }

    public async Task<User?> GetUser(
        Guid? userId = null,
        int? compatId = null,
        string? identifier = null,
        bool tracking = true,
        CancellationToken ct = default)
    {
        var specification = new GetUserSpecification(userId, compatId, identifier, tracking);
        var user = await _dbContext.Users
            .WithSpecification(specification)
            .SingleOrDefaultAsync(ct);

        return user;
    }
}