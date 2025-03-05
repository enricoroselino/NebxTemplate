namespace Modules.Identity.Application.Dtos;

public record UserDto(Guid Id, int? CompatId, string Username, string Email, string Fullname);