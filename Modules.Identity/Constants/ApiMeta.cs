using Shared.Models.ValueObjects;

namespace Modules.Identity.Constants;

internal static class ApiMeta
{
    public static readonly EndpointMeta Authentication = new EndpointMeta("Identity Auth", "auth");
}