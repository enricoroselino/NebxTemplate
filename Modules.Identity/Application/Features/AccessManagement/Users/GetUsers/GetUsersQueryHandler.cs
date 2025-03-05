using Ardalis.Specification.EntityFrameworkCore;
using BuildingBlocks.API.Models;
using Modules.Identity.Application.Dtos;
using Modules.Identity.Data;
using Modules.Identity.Data.Specifications;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Application.Features.AccessManagement.Users.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Response<GetUsersResponse>>
{
    private readonly AppIdentityDbContext _dbContext;

    public GetUsersQueryHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var userSpec = new GetUserSpecification(identifier: request.SearchTerm, tracking: true);
        var paginationSpec = new PaginationSpecification<User>(request.Pagination);

        var query = _dbContext.Users
            .WithSpecification(userSpec)
            .OrderBy(x => x.CreatedOn);

        var users = await query
            .WithSpecification(paginationSpec)
            .Select(x => new UserDto(x.Id, x.CompatId, x.UserName, x.Email, x.FullName))
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);
        var meta = new MetaBuilder()
            .AddPagination(request.Pagination, count)
            .Build();

        var responseDto = new GetUsersResponse(users);
        var response = Response.Build(responseDto, meta);
        return response;
    }
}