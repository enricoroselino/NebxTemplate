namespace Modules.Identity.Application.Features.AccessManagement.UpdateUserRoles;

public record UpdateUserRolesCommand(Guid UserId, List<Guid> RoleIds) : ICommand<Verdict>;
