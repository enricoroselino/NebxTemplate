using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Application.Features.AccessManagement.UpdateUserRoles;

public class UpdateUserRolesCommandHandler : ICommandHandler<UpdateUserRolesCommand, Verdict>
{
    private readonly IUserRepository _userRepository;
    private readonly AppIdentityDbContext _dbContext;

    public UpdateUserRolesCommandHandler(
        IUserRepository userRepository,
        AppIdentityDbContext dbContext)
    {
        _userRepository = userRepository;
        _dbContext = dbContext;
    }

    public async Task<Verdict> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var user = await _userRepository.GetUser(userId: request.UserId, tracking: true, ct: cancellationToken);
        if (user is null) return Verdict.NotFound("User not found");

        var currentRoles = await _dbContext.UserRoles
            .Where(u => u.UserId == request.UserId)
            .Select(x => x.RoleId)
            .ToListAsync(cancellationToken);

        var toRemove = currentRoles.Except(request.RoleIds).ToList();
        var toAdd = request.RoleIds.Except(currentRoles).ToList();

        var rolesAdd = await _dbContext.Roles
            .AsNoTracking()
            .Where(role => toAdd.Contains(role.Id))
            .Select(x => UserRole.Create(user, x))
            .ToListAsync(cancellationToken);

        var rolesRemove = await _dbContext.UserRoles
            .Where(x => toRemove.Contains(x.RoleId))
            .ToListAsync(cancellationToken);

        if (rolesAdd.Count > 0) await _dbContext.UserRoles.AddRangeAsync(rolesAdd, cancellationToken);
        if (rolesRemove.Count > 0) _dbContext.UserRoles.RemoveRange(rolesRemove);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return Verdict.Success();
    }
}