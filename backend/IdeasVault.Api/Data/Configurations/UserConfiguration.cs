using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdeasVault.Api.Data.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
  public override void Configure(EntityTypeBuilder<User> builder)
  {
    base.Configure(builder);

    builder.ToTable(t => t.HasCheckConstraint(
      "CK_Users_Username_NotBlank",
      "btrim(\"Username\") <> ''"));
    builder.Property(user => user.Username)
      .HasColumnType("citext")
      .HasMaxLength(50);
    builder.HasIndex(user => user.Username).IsUnique();

    builder.Property(user => user.PasswordHash).HasMaxLength(255);
    builder.Property(user => user.DisplayName).HasMaxLength(50);

    builder.ToTable(t =>
      {
        t.HasCheckConstraint("CK_Users_Username_NotBlank",
          "btrim(\"Username\") <> ''");
        t.HasCheckConstraint("CK_Users_Username_Length",
          "char_length(\"Username\") <= 50");   // backs up the citext/maxlength question
        t.HasCheckConstraint("CK_Users_DisplayName_NotBlank",
          "\"DisplayName\" IS NULL OR btrim(\"DisplayName\") <> ''");
      });

    builder.HasOne(user => user.Household)
      .WithMany(household => household.Users)
      .HasForeignKey(user => user.HouseholdId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}