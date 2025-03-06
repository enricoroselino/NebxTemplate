using Shared.Models.ValueObjects;

namespace Modules.Identity.Application.Features.AccessManagement.Users.GetUsers;

public record GetUsersQuery(string? SearchTerm, Pagination Pagination) : IQuery<Response<GetUsersResponse>>;