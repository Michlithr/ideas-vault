using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IdeasVault.Api.Data.Interceptors;

public class UpdatedAtInterceptor : SaveChangesInterceptor
{
  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken ct = default)
  {
    if (eventData.Context is { } ctx)
    {
      var now = DateTime.UtcNow;
      var updatedEntities = ctx.ChangeTracker.Entries<BaseEntity>()
        .Where(entity => entity.State is EntityState.Added or EntityState.Modified);

      foreach (var e in updatedEntities)
      {
        e.Entity.UpdatedAt = now;
      }
    }
    return base.SavingChangesAsync(eventData, result, ct);
  }
}