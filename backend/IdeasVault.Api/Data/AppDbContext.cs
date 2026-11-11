using IdeasVault.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdeasVault.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Household> Households => Set<Household>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Label> Labels => Set<Label>();
  public DbSet<Idea> Ideas => Set<Idea>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresExtension("citext");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}