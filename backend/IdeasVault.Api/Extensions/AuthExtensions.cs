using IdeasVault.Api.Auth;
using IdeasVault.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdeasVault.Api.Extensions;

public static class AuthExtensions
{
  public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
  {
    services.AddOptions<JwtOptions>()
      .Bind(config.GetSection("Jwt"))
      .ValidateDataAnnotations()
      .ValidateOnStart();

    services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

    return services;
  }
}