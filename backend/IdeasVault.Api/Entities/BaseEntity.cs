namespace IdeasVault.Api.Entities;

public abstract class BaseEntity
{
  public Guid Id { get; set; } = Guid.CreateVersion7();
  public DateTime UpdatedAt { get; set; }
}