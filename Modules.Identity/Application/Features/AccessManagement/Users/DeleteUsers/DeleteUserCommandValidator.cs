namespace Modules.Identity.Application.Features.AccessManagement.Users.DeleteUsers;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUsersCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEqual(Guid.Empty);
    }
}