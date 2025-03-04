using BuildingBlocks.API.Models.CQRS;
using Shared.Models.Responses;

namespace Modules.Identity.Features.GetClaims;

public record GetClaimsQuery(Guid UserId) : IQuery<Response<GetClaimsResponse>>;