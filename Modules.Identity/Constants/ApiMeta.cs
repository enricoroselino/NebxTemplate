using Shared.Models.ValueObjects;

namespace Modules.Identity.Constants;

internal static class ApiMeta
{
    public static readonly EndpointMeta Authentication = new EndpointMeta("Authentication", "auth");
    public static readonly EndpointMeta AccessManagement = new EndpointMeta("Access Management", "iam");
}