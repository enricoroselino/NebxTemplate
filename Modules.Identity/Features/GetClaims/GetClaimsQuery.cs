namespace Modules.Identity.Features.GetClaims;

public record GetClaimsQuery(Guid UserId) : IQuery<Response<GetClaimsResponse>>;