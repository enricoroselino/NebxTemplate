namespace Modules.Identity.Application.Features.AccessManagement.Roles.UpdateRoles;

public class UpdateRolesCommandValidator : AbstractValidator<UpdateRolesCommand>
{
    public UpdateRolesCommandValidator()
    {
        RuleFor(r => r.RoleId).NotEqual(Guid.Empty);
        RuleFor(r => r.Description).NotEmpty();
        RuleFor(r => r.Key).NotEmpty();
    }
}