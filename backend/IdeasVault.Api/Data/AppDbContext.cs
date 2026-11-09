using IdeasVault.Api.Data.Configurations;
using IdeasVault.Api.Entities;
using IdeasVault.Api.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdeasVault.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Household> Households => Set<Household>();
  public DbSet<User> Users => Set<User>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasPostgresExtension("citext");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}