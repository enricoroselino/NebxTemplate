namespace Modules.Identity.Features.Authentication.Login;

public record LoginCommand(string Identifier, string Password) : ICommand<Verdict<Response<LoginResponse>>>;