using Modules.Identity.Data;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Features.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Verdict>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(AppIdentityDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<Verdict> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = User.Create(request.Username, request.Email, request.Fullname);

        var existing = await _dbContext.Users
            .AsNoTracking()
            .Where(x =>
                x.NormalizedUserName == request.Username.ToUpperInvariant() ||
                x.NormalizedEmail == request.Email.ToUpperInvariant())
            .SingleOrDefaultAsync(cancellationToken);
        
        if (existing is not null) return Verdict.Conflict("User already exists.");
        
        var result = await _userManager.CreateAsync(newUser, request.Password);
        return !result.Succeeded ? Verdict.InternalError(result.GetErrors()) : Verdict.NoContent();
    }
}