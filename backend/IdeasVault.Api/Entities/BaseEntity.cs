using IdeasVault.Api.Entities.Interfaces;

namespace IdeasVault.Api.Entities;

public class BaseEntity : IHasUpdatedAt
{
  public Guid Id { get; set; } = Guid.CreateVersion7();
  public DateTime UpdatedAt { get; set; }
}