namespace Modules.Identity.Application.Features.AccessManagement.Roles.UpdateRoles;

public record UpdateRolesCommand(Guid RoleId, string Key, string Description) : ICommand<Verdict>;