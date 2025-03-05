namespace Modules.Identity.Application.Features.AccessManagement.Roles.DeleteRoles;

public class DeleteRolesCommandValidator : AbstractValidator<DeleteRolesCommand>
{
    public DeleteRolesCommandValidator()
    {
        RuleFor(x => x.RoleId).NotEqual(Guid.Empty);
    }
}