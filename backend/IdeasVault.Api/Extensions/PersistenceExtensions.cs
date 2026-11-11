using Npgsql;
using Microsoft.EntityFrameworkCore;
using IdeasVault.Api.Data;
using IdeasVault.Api.Data.Interceptors;

namespace IdeasVault.Api.Extensions;

public static class PersistenceExtensions
{
  public static IServiceCollection AddPersistence(
    this IServiceCollection services, IConfiguration config)
  {
    var connectionString = BuildConnectionString(config.GetSection("Db"));

    services.AddSingleton<UpdatedAtInterceptor>();
    services.AddDbContext<AppDbContext>((sp, options) => options
      .UseNpgsql(connectionString)
      .AddInterceptors(sp.GetRequiredService<UpdatedAtInterceptor>()));

    return services;
  }

  private static string BuildConnectionString(IConfigurationSection db) => new NpgsqlConnectionStringBuilder
  {
    Host = db["Host"] ?? throw new InvalidOperationException("Db:Host is not configured"),
    Port = db.GetValue<int>("Port"),
    Database = db["Name"],
    Username = db["User"],
    Password = db["Password"],
  }.ConnectionString;
}