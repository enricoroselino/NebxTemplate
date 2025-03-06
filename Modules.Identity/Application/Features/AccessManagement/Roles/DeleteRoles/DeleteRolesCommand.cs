namespace Modules.Identity.Application.Features.AccessManagement.Roles.DeleteRoles;

public record DeleteRolesCommand(Guid RoleId) : ICommand<Verdict>;
