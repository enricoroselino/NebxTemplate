namespace Modules.Identity.Features.Login;

public record LoginCommand(string Identifier, string Password) : ICommand<Verdict<Response<LoginResponse>>>;