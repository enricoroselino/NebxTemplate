namespace Modules.Identity.Features.Authentication.ChangePassword;

public record ChangePasswordCommand(
    Guid UserId,
    string OldPassword,
    string Password,
    string PasswordConfirmation,
    Guid TokenId) : ICommand<Verdict>;