namespace IdeasVault.Api.Entities;

public class Household : BaseEntity
{
  public required string Name { get; set; }
  public List<User> Users { get; } = [];
  public List<Label> Labels { get; } = [];
  public List<Idea> Ideas { get; } = [];
}