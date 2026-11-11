namespace IdeasVault.Api.Entities;

public class Idea : BaseEntity
{
  public required string Title { get; set; }
  public Uri? Url { get; set; }
  public string? Note { get; set; }
  public Uri? ThumbnailUrl { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required Guid AuthorId { get; set; }
  public User Author { get; set; } = null!;
  public required Guid HouseholdId { get; set; }
  public Household Household { get; set; } = null!;
  public List<Label> Labels { get; } = [];
}