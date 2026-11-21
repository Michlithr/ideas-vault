using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace IdeasVault.Api.Endpoints;

public static class HealthEnpoints
{
  public static IEndpointRouteBuilder MapHealth(this IEndpointRouteBuilder enpoints)
  {
    var group = enpoints.MapGroup("/health");

    group.MapHealthChecks("/live", LiveOptions);
    group.MapHealthChecks("/ready", ReadyOptions);

    return enpoints;
  }

  private static readonly HealthCheckOptions LiveOptions = new() { Predicate = _ => false };
  private static readonly HealthCheckOptions ReadyOptions = new() { Predicate = check => check.Tags.Contains("ready") };
}