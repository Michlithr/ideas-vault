using IdeasVault.Api.Entities;
using IdeasVault.Api.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdeasVault.Api.Tests.Auth;

public class AuthExtensionsTests
{
  private static readonly User TestUser = new()
  {
    Username = "tester",
    PasswordHash = "",
    HouseholdId = Guid.NewGuid()
  };

  private static IPasswordHasher<User> CreateHasher()
  {
    var services = new ServiceCollection();

    services.AddAuth(new ConfigurationBuilder().Build());

    return services.BuildServiceProvider()
      .GetRequiredService<IPasswordHasher<User>>();
  }

  [Fact]
  public void Verify_succeeds_for_the_correct_password()
  {
    var hasher = CreateHasher();
    var hash = hasher.HashPassword(TestUser, "correctPassword");

    var result = hasher.VerifyHashedPassword(TestUser, hash, "correctPassword");

    Assert.Equal(PasswordVerificationResult.Success, result);
  }

  [Fact]
  public void Verify_fails_for_the_wrong_password()
  {
    var hasher = CreateHasher();
    var hash = hasher.HashPassword(TestUser, "correctPassword");

    var result = hasher.VerifyHashedPassword(TestUser, hash, "wrongPassword");

    Assert.Equal(PasswordVerificationResult.Failed, result);
  }

  [Fact]
  public void Hashing_the_same_password_twice_gives_different_hashes()
  {
    const string PASSWORD = "pass1";
    var hasher = CreateHasher();

    Assert.NotEqual(
      hasher.HashPassword(TestUser, PASSWORD),
      hasher.HashPassword(TestUser, PASSWORD)
    );
  }
}