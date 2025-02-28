using BuildingBlocks.API.Models.CQRS;
using Modules.Identity.Data;
using Shared.Verdict;

namespace Modules.Identity.Features.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Verdict>
{
    private readonly AppIdentityDbContext _dbContext;

    public RegisterCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Verdict> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Verdict.Success());
    }
}