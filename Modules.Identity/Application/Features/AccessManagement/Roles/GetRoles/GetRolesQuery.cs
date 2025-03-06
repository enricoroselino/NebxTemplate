using Shared.Models.ValueObjects;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.GetRoles;

public record GetRolesQuery(string? SearchTerm, Pagination Pagination) : IQuery<Response<GetRolesResponse>>;
