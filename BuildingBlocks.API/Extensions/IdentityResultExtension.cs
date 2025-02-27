using Microsoft.AspNetCore.Identity;

namespace BuildingBlocks.API.Extensions;

public static class IdentityResultExtension
{
    public static string GetErrors(this IdentityResult identityResult)
    {
        return string.Join(", ", identityResult.Errors.Select(e => e.Description));
    }
}