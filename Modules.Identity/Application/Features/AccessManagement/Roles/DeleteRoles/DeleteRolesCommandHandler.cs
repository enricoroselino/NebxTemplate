using Modules.Identity.Data;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.DeleteRoles;

public class DeleteRolesCommandHandler : ICommandHandler<DeleteRolesCommand, Verdict>
{
    private readonly AppIdentityDbContext _dbContext;

    public DeleteRolesCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Verdict> Handle(DeleteRolesCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var role = await _dbContext.Roles.FindAsync([request.RoleId], cancellationToken);
        if (role is null) return Verdict.NoContent();

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return Verdict.NoContent();
    }
}