namespace IdeasVault.Api.Entities;

public class Label : BaseEntity
{
  public required string Name { get; set; }
  public required Guid HouseholdId { get; set; }
  public Household Household { get; set; } = null!;
  public List<Idea> Ideas { get; } = [];
}