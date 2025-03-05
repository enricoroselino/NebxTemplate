using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Services;
using Shared;

namespace Modules.Identity.Application.Features.Authentication.Impersonate;

public class ImpersonateCommandHandler : ICommandHandler<ImpersonateCommand, Verdict<Response<ImpersonateResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenServices _tokenServices;
    private readonly AppIdentityDbContext _dbContext;

    public ImpersonateCommandHandler(
        IUserRepository userRepository,
        ITokenServices tokenServices,
        AppIdentityDbContext dbContext)
    {
        _userRepository = userRepository;
        _tokenServices = tokenServices;
        _dbContext = dbContext;
    }

    public async Task<Verdict<Response<ImpersonateResponse>>> Handle(
        ImpersonateCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var targetUser = await _userRepository.GetUser(
            userId: request.TargetUserId,
            tracking: false,
            ct: cancellationToken);

        if (targetUser is null) return Verdict.NotFound("Target user not found");

        var impersonatorUser = await _userRepository.GetUser(
            userId: request.ImpersonatorUserId,
            tracking: false,
            ct: cancellationToken);

        if (impersonatorUser is null) return Verdict.NotFound("Impersonator user not found");

        var claims = _userRepository
            .GetInformationClaims(targetUser)
            .Concat([new Claim(CustomClaim.ImpersonatorUserId, impersonatorUser.Id.ToString())])
            .ToList();

        // revoke current user token
        var revokeResult = await _tokenServices.RevokeToken(
            impersonatorUser,
            request.ImpersonatorTokenId,
            cancellationToken);

        if (!revokeResult.IsSuccess) return Verdict.InternalError(revokeResult.ErrorMessage);

        // track impersonated token
        var issueResult = await _tokenServices.IssueToken(targetUser, claims, cancellationToken);
        if (!issueResult.IsSuccess) return Verdict.InternalError(issueResult.ErrorMessage);

        await transaction.CommitAsync(cancellationToken);

        var responseDto = new ImpersonateResponse(issueResult.Value.AccessToken, issueResult.Value.RefreshToken);
        var response = Response.Build(responseDto);
        return Verdict.Success(response);
    }
}