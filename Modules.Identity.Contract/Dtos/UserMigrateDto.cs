namespace Modules.Identity.Contract.Dtos;

public record UserMigrateDto(string Username, string Password, string Email, string FullName, int CompatId);