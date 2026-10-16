using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using OjitexSystem_Backend.Dtos.Auth;
using OjitexSystem_Backend.Services.Auth;

namespace OjitexSystem_Backend.Tests;

public sealed class AuthenticationTests
{
    [Fact]
    public async Task LoginReturnsDirectAndInheritedAccessAndUsesBusinessTime()
    {
        using var fixture = new AuthTestFixture();
        var user = fixture.AddUser();
        fixture.AddGrant("alice", "*", "C000000005", false);
        fixture.AddGrant("*", "USER", "C000000006", true);

        var response = await fixture.Authentication.LoginAsync(new LoginRequest(" alice ", "original-password"));

        Assert.NotNull(response);
        Assert.Equal("alice", response.User.UserId);
        Assert.Equal(2, response.User.Categories.Count);
        Assert.Equal(new DateTime(2026, 2, 1, 1, 0, 0), user.LastLoginDate);
        Assert.Equal(fixture.Clock.GetUtcNow().UtcDateTime.AddHours(1), response.ExpiresAt);
        Assert.Equal(response.ExpiresAt, new JwtSecurityTokenHandler().ReadJwtToken(response.AccessToken).ValidTo);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task LockedOrDeletedUsersCannotLoginOrRetrieveProfile(bool locked, bool deleted)
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser(locked: locked, deleted: deleted);
        Assert.Null(await fixture.Authentication.LoginAsync(new LoginRequest("alice", "original-password")));
        Assert.Null(await fixture.Authentication.GetCurrentUserAsync("alice"));
    }

    [Theory]
    [InlineData("bad-password")]
    [InlineData("")]
    public async Task WrongPasswordDoesNotUpdateLoginDate(string password)
    {
        using var fixture = new AuthTestFixture();
        var user = fixture.AddUser();
        Assert.Null(await fixture.Authentication.LoginAsync(new LoginRequest("alice", password)));
        Assert.Null(user.LastLoginDate);
        Assert.Empty(fixture.UnitOfWork.Events);
    }

    [Fact]
    public async Task MalformedLegacyPasswordHashFailsWithoutThrowing()
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser().Password = "not-an-identity-hash";
        Assert.Null(await fixture.Authentication.LoginAsync(new LoginRequest("alice", "original-password")));
    }

    [Fact]
    public async Task PasswordChangePreservesHistoryAndRejectsOldPassword()
    {
        using var fixture = new AuthTestFixture();
        var user = fixture.AddUser();
        var oldHash = user.Password;
        Assert.True(await fixture.Authentication.ChangePasswordAsync("alice", new ChangePasswordRequest("original-password", "new-password")));
        Assert.Equal(oldHash, user.PastPassword);
        Assert.Equal(PasswordVerificationResult.Success, fixture.Hasher.VerifyHashedPassword(user, user.Password!, "new-password"));
        Assert.Null(await fixture.Authentication.LoginAsync(new LoginRequest("alice", "original-password")));
    }

    [Fact]
    public async Task PasswordChangeWithIncorrectCurrentPasswordDoesNotWrite()
    {
        using var fixture = new AuthTestFixture();
        var user = fixture.AddUser();
        var hash = user.Password;
        Assert.False(await fixture.Authentication.ChangePasswordAsync("alice", new ChangePasswordRequest("wrong-password", "new-password")));
        Assert.Equal(hash, user.Password);
        Assert.Empty(fixture.UnitOfWork.Events);
    }

    [Fact]
    public async Task LegacyMigrationSkipsValidHashesAndResetsOnlyLegacyActiveUsers()
    {
        using var fixture = new AuthTestFixture();
        var valid = fixture.AddUser("valid");
        var validHash = valid.Password;
        var legacy = fixture.AddUser("legacy", locked: true);
        legacy.Password = "legacy-hash";
        var deleted = fixture.AddUser("deleted", deleted: true);
        deleted.Password = "deleted-hash";
        await fixture.Context.SaveChangesAsync();
        var migration = new LegacyPasswordMigrationService(fixture.Users, fixture.UnitOfWork, fixture.Clock, fixture.Hasher);
        Assert.Equal(1, await migration.ResetLegacyPasswordsAsync());
        Assert.Equal(validHash, valid.Password);
        Assert.Equal("deleted-hash", deleted.Password);
        Assert.Equal(0, legacy.UserLockFlag);
        Assert.Equal(PasswordVerificationResult.Success, fixture.Hasher.VerifyHashedPassword(legacy, legacy.Password!, "123456"));
    }
}
