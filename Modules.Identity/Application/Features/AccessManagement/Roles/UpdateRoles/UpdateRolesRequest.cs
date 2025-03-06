namespace Modules.Identity.Application.Features.AccessManagement.Roles.UpdateRoles;

public record UpdateRolesRequest(Guid RoleId, string Key, string Description);