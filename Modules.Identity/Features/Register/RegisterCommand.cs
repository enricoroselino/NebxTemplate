namespace Modules.Identity.Features.Register;

public record RegisterCommand(
    string Username,
    string Password,
    string PasswordConfirmation,
    string Email,
    string Fullname) : ICommand<Verdict>;