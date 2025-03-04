using Ardalis.Specification;
using Modules.Identity.Domain.Models;

namespace Modules.Identity.Data.Specifications;

public sealed class GetUserSpecification : Specification<User>
{
    public GetUserSpecification(
        Guid? userId = null,
        int? compatId = null,
        string? identifier = null,
        bool tracking = true)
    {
        if (!tracking) Query.AsNoTracking();

        if (userId.HasValue)
        {
            Query.Where(u => u.Id == userId);
        }
        else if (compatId.HasValue)
        {
            Query.Where(u => u.CompatId == compatId);
        }
        else if (!string.IsNullOrWhiteSpace(identifier))
        {
            Query.Where(u =>
                u.NormalizedUserName == identifier.ToUpperInvariant() ||
                u.NormalizedEmail == identifier.ToUpperInvariant());
        }

        Query.Where(x => x.IsActive);
    }
}