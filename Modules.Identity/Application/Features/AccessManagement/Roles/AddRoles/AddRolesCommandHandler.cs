using Modules.Identity.Data;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.AddRoles;

public class AddRolesCommandHandler : IRequestHandler<AddRolesCommand, Verdict<Response<Guid>>>
{
    private readonly AppIdentityDbContext _dbContext;

    public AddRolesCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Verdict<Response<Guid>>> Handle(AddRolesCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Roles
            .AnyAsync(role => role.NormalizedName == request.Key.ToUpperInvariant(), cancellationToken);
        if (exists) return Verdict.Conflict("Role already exists");
        
        var newRole = Role.Create(request.Key, request.Description);
        await _dbContext.Roles.AddAsync(newRole, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        var response = Response.Build(newRole.Id);
        return Verdict.Created(response);
    }
}