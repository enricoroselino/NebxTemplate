namespace Modules.Identity.Features.Register;

public record RegisterRequest(
    string Username,
    string Password,
    string PasswordConfirmation,
    string Email,
    string FullName);