namespace Modules.Identity.Application.Features.Authentication.Register;

public record RegisterCommand(
    string Username,
    string Password,
    string PasswordConfirmation,
    string Email,
    string Fullname) : ICommand<Verdict>;