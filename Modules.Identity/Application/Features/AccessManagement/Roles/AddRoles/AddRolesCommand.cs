namespace Modules.Identity.Application.Features.AccessManagement.Roles.AddRoles;

public record AddRolesCommand(string Key, string Description) : ICommand<Verdict<Response<Guid>>>;