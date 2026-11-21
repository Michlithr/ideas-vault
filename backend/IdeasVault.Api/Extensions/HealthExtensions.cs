using IdeasVault.Api.Data;

namespace IdeasVault.Api.Extensions;

public static class HealthExtensions
{
  public static IServiceCollection AddHealth(this IServiceCollection services)
  {
    services.AddHealthChecks().AddDbContextCheck<AppDbContext>(tags: ["ready"]);

    return services;
  }
}