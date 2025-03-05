namespace Modules.Identity.Application.Features.AccessManagement.UpdateUserRoles;

public class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>
{
    public UpdateUserRolesCommandValidator()
    {
        RuleFor(p => p.UserId).NotEqual(Guid.Empty);
        RuleForEach(x => x.RoleIds).NotEqual(Guid.Empty);
    }
}