using Modules.Identity.Application.Dtos;

namespace Modules.Identity.Application.Features.AccessManagement.Users.GetUsers;

public record GetUsersResponse(List<UserDto> Users) : IQuery<GetUsersResponse>;
