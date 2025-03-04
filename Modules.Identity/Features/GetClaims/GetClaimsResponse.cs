using Shared.Models.ValueObjects;

namespace Modules.Identity.Features.GetClaims;

public record GetClaimsResponse(List<ClaimEntity> Claims);