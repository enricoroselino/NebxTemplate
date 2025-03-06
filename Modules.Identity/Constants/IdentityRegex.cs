using Shared.Models.ValueObjects;

namespace Modules.Identity.Constants;

internal static class IdentityRegex
{
    public static readonly RegexRule ValidUserName = new RegexRule(
        @"^[a-z0-9](?!.*[_.]{2})(?!.*[_.]$)[a-z0-9_.]*$",
        "Username must start and end with a letter or digit, cannot contain consecutive underscores or periods, and can only include letters, digits, underscores, and periods."
    );

    public static readonly RegexRule ValidPassword = new RegexRule(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]*$",
        "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*), and can only include letters, digits, and the specified special characters."
    );

    public static readonly RegexRule ValidFullName = new RegexRule(
        @"^[A-Za-z]+(?:[ '\s][A-Za-z]+)*$",
        "Full name must contain only letters, spaces, and apostrophes, and cannot have double spaces or other symbols."
    );
}