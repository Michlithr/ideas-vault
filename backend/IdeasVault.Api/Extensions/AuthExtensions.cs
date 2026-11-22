using IdeasVault.Api.Auth;

namespace IdeasVault.Api.Extensions;

public static class AuthExtensions
{
  public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
  {
    services.AddOptions<JwtOptions>()
      .Bind(config.GetSection("Jwt"))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    return services;
  }
}