using BuildingBlocks.API.Models.CQRS;
using Modules.Identity.Domain.Services;
using Shared.Models.Responses;
using Shared.Models.ValueObjects;

namespace Modules.Identity.Features.GetClaims;

public class GetClaimsQueryHandler : IQueryHandler<GetClaimsQuery, Response<GetClaimsResponse>>
{
    private readonly IClaimServices _claimServices;

    public GetClaimsQueryHandler(IClaimServices claimServices)
    {
        _claimServices = claimServices;
    }

    public async Task<Response<GetClaimsResponse>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
    {
        var claims = await _claimServices.GetClaims(request.UserId, cancellationToken);
        var responseDto = new GetClaimsResponse(claims.Select(x => new ClaimEntity(x.Type, x.Value)).ToList());
        var response = Response.Build(responseDto);
        return response;
    }
}