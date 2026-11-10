namespace IdeasVault.Api.Entities;

public class User : BaseEntity
{
  public required string Username { get; set; }
  public required string PasswordHash { get; set; }
  public string? DisplayName { get; set; }
  public required Guid HouseholdId { get; set; }
  public Household Household { get; set; } = null!;
  public List<Idea> Ideas { get; } = [];
}