using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdeasVault.Api.Data.Configurations;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
  where T : BaseEntity
{
  public virtual void Configure(EntityTypeBuilder<T> builder)
  {
    builder.Property(e => e.Id).ValueGeneratedNever();
  }
}