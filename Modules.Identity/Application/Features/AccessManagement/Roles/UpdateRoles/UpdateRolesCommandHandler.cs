using Modules.Identity.Data;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.UpdateRoles;

public class UpdateRolesCommandHandler : ICommandHandler<UpdateRolesCommand, Verdict>
{
    private readonly AppIdentityDbContext _dbContext;

    public UpdateRolesCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Verdict> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role is null) return Verdict.NotFound("Role not found");
        
        role.Update(request.Key, request.Description);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Verdict.Success();
    }
}