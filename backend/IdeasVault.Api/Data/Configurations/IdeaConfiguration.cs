using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdeasVault.Api.Data.Configurations;

public class IdeaConfiguration : BaseEntityConfiguration<Idea>
{
  public override void Configure(EntityTypeBuilder<Idea> builder)
  {
    base.Configure(builder);

    builder.Property(idea => idea.Title).HasColumnType("citext");

    builder.ToTable(table =>
      {
        table.HasCheckConstraint("CK_Ideas_Title_NotBlank",
          "btrim(\"Title\") <> ''");
      });

    builder.HasOne(idea => idea.Author)
      .WithMany(user => user.Ideas)
      .HasForeignKey(idea => idea.AuthorId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(idea => idea.Household)
      .WithMany(household => household.Ideas)
      .HasForeignKey(idea => idea.HouseholdId)
      .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(idea => idea.Labels)
      .WithMany(label => label.Ideas)
      .UsingEntity(join => join.ToTable("IdeaLabels"));
  }
}