namespace Modules.Identity.Application.Features.GetClaims;

public record GetClaimsQuery(Guid UserId) : IQuery<Response<GetClaimsResponse>>;