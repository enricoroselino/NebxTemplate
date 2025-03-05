using Modules.Identity.Application.Dtos;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.GetRoles;

public record GetRolesResponse(List<RoleDto> Roles);
