namespace Modules.Identity.Features.Authentication.Register;

public record RegisterRequest(
    string Username,
    string Password,
    string PasswordConfirmation,
    string Email,
    string FullName);