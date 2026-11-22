using System.ComponentModel.DataAnnotations;

namespace IdeasVault.Api.Auth;

public class JwtOptions
{
  [Required, MinLength(32)]
  public string Secret { get; init; } = "";

  [Required]
  public string Issuer { get; init; } = "";

  [Required]
  public string Audience { get; init; } = "";

  [Range(1, 1440)]
  public int LifetimeMinutes { get; init; }
}