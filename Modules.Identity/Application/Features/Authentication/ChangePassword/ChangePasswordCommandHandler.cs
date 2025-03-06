using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Models;
using Modules.Identity.Domain.Services;

namespace Modules.Identity.Application.Features.Authentication.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, Verdict>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly AppIdentityDbContext _dbContext;
    private readonly ITokenServices _tokenServices;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        UserManager<User> userManager,
        AppIdentityDbContext dbContext,
        ITokenServices tokenServices)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _dbContext = dbContext;
        _tokenServices = tokenServices;
    }

    public async Task<Verdict> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var user = await _userRepository.GetUser(userId: request.UserId, tracking: true, ct: cancellationToken);
        if (user is null) return Verdict.Unauthorized();

        var changeResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);
        if (!changeResult.Succeeded) return Verdict.InternalError(changeResult.GetErrors());

        var revokeResult = await _tokenServices.RevokeToken(user, request.TokenId, cancellationToken);
        if (!revokeResult.IsSuccess) return Verdict.InternalError(revokeResult.ErrorMessage);

        await transaction.CommitAsync(cancellationToken);
        return Verdict.Success();
    }
}