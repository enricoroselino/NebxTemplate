using Modules.Identity.Data;
using Modules.Identity.Data.Repository;
using Modules.Identity.Domain.Services;

namespace Modules.Identity.Application.Features.AccessManagement.Users.DeleteUsers;

public class DeleteUsersCommandHandler : ICommandHandler<DeleteUsersCommand, Verdict>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly ITokenServices _tokenServices;

    public DeleteUsersCommandHandler(
        AppIdentityDbContext dbContext,
        IUserRepository userRepository,
        ITokenServices tokenServices)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _tokenServices = tokenServices;
    }

    public async Task<Verdict> Handle(DeleteUsersCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var user = await _userRepository.GetUser(userId: request.UserId, tracking: true, ct: cancellationToken);
        if (user is null) return Verdict.NotFound("User not found");

        _ = await _tokenServices.RevokeTokenByUser(user, cancellationToken);

        user.Deactivate();
        await _dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return Verdict.NoContent();
    }
}