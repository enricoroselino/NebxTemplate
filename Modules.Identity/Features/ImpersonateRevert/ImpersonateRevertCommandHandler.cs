using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Services;

namespace Modules.Identity.Features.ImpersonateRevert;

public class ImpersonateRevertCommandHandler
    : ICommandHandler<ImpersonateRevertCommand, Verdict<Response<ImpersonateRevertResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenServices _tokenServices;
    private readonly AppIdentityDbContext _dbContext;

    public ImpersonateRevertCommandHandler(
        IUserRepository userRepository,
        ITokenServices tokenServices,
        AppIdentityDbContext dbContext)
    {
        _userRepository = userRepository;
        _tokenServices = tokenServices;
        _dbContext = dbContext;
    }

    public async Task<Verdict<Response<ImpersonateRevertResponse>>> Handle(
        ImpersonateRevertCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var impersonatorUser = await _userRepository.GetUser(
            userId: request.ImpersonatorUserId,
            tracking: false,
            ct: cancellationToken);

        if (impersonatorUser is null) return Verdict.NotFound("Impersonator user not found");

        var targetUser = await _userRepository.GetUser(
            userId: request.TargetUserId,
            tracking: false,
            ct: cancellationToken);

        if (targetUser is null) return Verdict.NotFound("Target user not found");

        // revoke impersonated target token
        var revokeResult = await _tokenServices.RevokeToken(
            targetUser,
            request.TargetTokenId,
            cancellationToken);

        if (!revokeResult.IsSuccess) return Verdict.InternalError(revokeResult.ErrorMessage);

        // track reverted impersonator user token
        var claims = _userRepository.GetInformationClaims(impersonatorUser);
        var issueResult = await _tokenServices.IssueToken(impersonatorUser, claims, cancellationToken);
        if (!issueResult.IsSuccess) return Verdict.InternalError(issueResult.ErrorMessage);

        await transaction.CommitAsync(cancellationToken);

        var responseDto = new ImpersonateRevertResponse(issueResult.Value.AccessToken, issueResult.Value.RefreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}