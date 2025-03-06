namespace Modules.Identity.Application.Features.AccessManagement.UpdateUserRoles;

public record UpdateUserRolesRequest(Guid UserId, List<Guid> RoleIds);