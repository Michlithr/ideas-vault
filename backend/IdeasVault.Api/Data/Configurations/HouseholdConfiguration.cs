using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdeasVault.Api.Data.Configurations;

public class HouseholdConfiguration : BaseEntityConfiguration<Household>
{
  public override void Configure(EntityTypeBuilder<Household> builder)
  {
    base.Configure(builder);

    builder.Property(household => household.Name).HasColumnType("citext");
    builder.HasIndex(household => household.Name).IsUnique();
    builder.Property(household => household.Name).HasMaxLength(50);
  }
}