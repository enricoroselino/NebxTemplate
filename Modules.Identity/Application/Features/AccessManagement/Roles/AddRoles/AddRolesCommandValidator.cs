namespace Modules.Identity.Application.Features.AccessManagement.Roles.AddRoles;

public class AddRolesValidator : AbstractValidator<AddRolesCommand>
{
    public AddRolesValidator()
    {
        RuleFor(c => c.Key).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
    }
}