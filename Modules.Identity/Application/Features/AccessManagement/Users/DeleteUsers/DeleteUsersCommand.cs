namespace Modules.Identity.Application.Features.AccessManagement.Users.DeleteUsers;

public record DeleteUsersCommand(Guid UserId) : ICommand<Verdict>;