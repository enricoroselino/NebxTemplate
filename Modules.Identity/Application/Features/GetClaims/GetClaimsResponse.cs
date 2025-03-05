using Shared.Models.ValueObjects;

namespace Modules.Identity.Application.Features.GetClaims;

public record GetClaimsResponse(List<ClaimEntity> Claims);