using System.Security.Claims;
using BuildingBlocks.API.Configurations;
using BuildingBlocks.API.Services;
using BuildingBlocks.API.Services.JwtManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Models;
using Modules.Identity.Domain.Services;
using Modules.Identity.Infrastructure;

namespace Modules.Identity;

public static class IdentityModuleSetup
{
    public static void AddIdentityModule(this IServiceCollection services)
    {
        services.AddModuleSetup(typeof(IdentityModuleSetup).Assembly);

        services.AddDbContext<AppIdentityDbContext>((provider, builder) =>
        {
            var interceptors = provider.GetServices<ISaveChangesInterceptor>();
            builder.AddInterceptors(interceptors);
        });

        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;

                // managed via fluent validation
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                options.ClaimsIdentity.UserNameClaimType = JwtRegisteredClaimNames.PreferredUsername;
                options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
                options.ClaimsIdentity.EmailClaimType = JwtRegisteredClaimNames.Email;
            })
            .AddRoles<Role>() // AddRoles should be at the top
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddRoleManager<RoleManager<Role>>()
            .AddUserManager<UserManager<User>>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();

        services.AddScoped<IHasher, BcryptHasher>();
        services.AddScoped<IPasswordHasher<User>, BcryptPasswordHasher>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IClaimServices, ClaimServices>();
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<ITokenServices, TokenServices>();
    }

    public static void UseIdentityModuleMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<RevokedTokenMiddleware>();
        app.UseMiddleware<ClaimsTransformationMiddleware>();
    }

    public static void UseIdentityModule(this WebApplication app)
    {
    }
}