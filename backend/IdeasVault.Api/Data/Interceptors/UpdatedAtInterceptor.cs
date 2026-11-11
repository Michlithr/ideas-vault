using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IdeasVault.Api.Data.Interceptors;

public sealed class UpdatedAtInterceptor : SaveChangesInterceptor
{
  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    ApplyTimestamps(eventData.Context);
    return base.SavingChanges(eventData, result);
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken ct = default)
  {
    ApplyTimestamps(eventData.Context);
    return base.SavingChangesAsync(eventData, result, ct);
  }

  private static void ApplyTimestamps(DbContext? ctx)
  {
    if (ctx is null) return;

    var now = DateTime.UtcNow;
    var updatedEntities = ctx.ChangeTracker.Entries<BaseEntity>()
      .Where(entity => entity.State is EntityState.Added or EntityState.Modified);

    foreach (var entity in updatedEntities)
    {
      entity.Entity.UpdatedAt = now;
    }
  }
}