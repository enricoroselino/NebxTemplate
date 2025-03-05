using Ardalis.Specification.EntityFrameworkCore;
using BuildingBlocks.API.Models;
using Modules.Identity.Application.Dtos;
using Modules.Identity.Data;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Application.Features.AccessManagement.Roles.GetRoles;

public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, Response<GetRolesResponse>>
{
    private readonly AppIdentityDbContext _dbContext;

    public GetRolesQueryHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<GetRolesResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var paginationSpec = new PaginationSpecification<Role>(request.Pagination);

        var query = _dbContext.Roles
            .AsNoTracking()
            .OrderBy(r => r.CreatedOn)
            .WithSpecification(paginationSpec);

        var roles = await query
            .Select(r => new RoleDto(r.Id, r.Name, r.Description))
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);
        var meta = new MetaBuilder()
            .AddPagination(request.Pagination, count)
            .Build();

        var responseDto = new GetRolesResponse(roles);
        var response = Response.Build(responseDto, meta);
        return response;
    }
}