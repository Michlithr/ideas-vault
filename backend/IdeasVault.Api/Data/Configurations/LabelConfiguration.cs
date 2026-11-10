using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdeasVault.Api.Data.Configurations;

public class LabelConfiguration : BaseEntityConfiguration<Label>
{
  public override void Configure(EntityTypeBuilder<Label> builder)
  {
    base.Configure(builder);

    builder.Property(label => label.Name).HasColumnType("citext");
    builder.HasIndex(label => new { label.HouseholdId, label.Name }).IsUnique();

    builder.ToTable(table =>
      {
        table.HasCheckConstraint("CK_Labels_Name_NotBlank",
          "btrim(\"Name\") <> ''");
      });

    builder.HasOne(label => label.Household)
      .WithMany(household => household.Labels)
      .HasForeignKey(label => label.HouseholdId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}