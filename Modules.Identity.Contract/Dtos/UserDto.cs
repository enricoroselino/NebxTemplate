namespace Modules.Identity.Contract.Dtos;

public record UserDto(Guid UserId, int? CompatId, string Name, string Email);