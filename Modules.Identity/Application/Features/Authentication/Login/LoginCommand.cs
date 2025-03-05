namespace Modules.Identity.Application.Features.Authentication.Login;

public record LoginCommand(string Identifier, string Password) : ICommand<Verdict<Response<LoginResponse>>>;