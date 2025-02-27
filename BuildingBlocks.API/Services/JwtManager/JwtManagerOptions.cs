using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.API.Services.JwtManager;

public class JwtManagerOptions
{
    public const string Section = nameof(JwtManagerOptions);
    private const int KeyLength = 64;

    [Required(ErrorMessage = "The Key is required.")]
    [StringLength(KeyLength, MinimumLength = KeyLength)]
    public string Key { get; init; } = string.Empty;

    [Required(ErrorMessage = "A ValidIssuer is required.")]
    public string ValidIssuer { get; init; } = string.Empty;

    [Required(ErrorMessage = "At least one ValidAudience is required.")]
    [MinLength(1)]
    public List<string> ValidAudiences { get; init; } = [];

    [Required(ErrorMessage = "The TokenSpan is required.")]
    public TimeSpan TokenSpan { get; init; }

    [Required(ErrorMessage = "The RefreshTokenSpan is required.")]
    public TimeSpan RefreshTokenSpan { get; init; }

    [Required(ErrorMessage = "The GracePeriodSpan is required.")]
    public TimeSpan GracePeriodSpan { get; init; }
}