using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Dtos.Admin;
using OjitexSystem_Backend.Services.Admin;

namespace OjitexSystem_Backend.Tests;

public sealed class AdminUserTests
{
    [Fact]
    public async Task SingleUserReadIsFilteredAndIncludesBothAccessSources()
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser();
        fixture.AddUser("bob");
        fixture.AddGrant("alice", "*", "C000000005", false);
        fixture.AddGrant("*", "USER", "C000000006", true);
        var result = Assert.Single(await fixture.AdminUsers.GetUsersAsync("alice"));
        Assert.Equal("alice", result.UserId);
        Assert.Equal(2, result.Categories.Count);
        Assert.Equal(["C000000005"], result.DirectCategoryIds);
        Assert.Null(await fixture.Admin.GetUserAsync("unknown"));
    }

    [Fact]
    public async Task UpdatingUnchangedRolesAndCategoriesDoesNotDuplicateTrackedKeys()
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser();
        fixture.AddGrant("alice", "*", "C000000005", false);
        fixture.AddGrant("*", "USER", "C000000006", true);
        var result = await fixture.Admin.UpdateUserAsync("alice", new UpdateUserRequest(" Nguyen ", " An ", [" USER ", "USER"], ["C000000005"]));
        Assert.NotNull(result);
        Assert.Equal("Nguyen", result.UserFamilyName);
        Assert.Single(await fixture.Context.IeUserRoles.Where(role => role.UserId == "alice").ToListAsync());
        Assert.Equal(2, await fixture.Context.IeCategoryAuths.CountAsync());
        Assert.Equal(IsolationLevel.Serializable, fixture.UnitOfWork.Isolation);
        Assert.Equal(["begin", "save", "commit", "dispose"], fixture.UnitOfWork.Events);
    }

    [Fact]
    public async Task UpdatingDirectGrantsDoesNotDeleteInheritedGrants()
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser();
        fixture.AddGrant("alice", "*", "C000000005", false);
        fixture.AddGrant("*", "USER", "C000000006", true);
        var result = await fixture.Admin.UpdateUserAsync("alice", new UpdateUserRequest(null, null, ["USER"], []));
        Assert.NotNull(result);
        Assert.Empty(result.DirectCategoryIds);
        Assert.Equal("C000000006", Assert.Single(result.Categories).CategoryId);
        Assert.Equal(1, await fixture.Context.IeCategoryAuths.CountAsync());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task LastActiveAdminCannotBeDeletedOrDemoted(bool delete)
    {
        using var fixture = new AuthTestFixture();
        var user = fixture.AddUser(admin: true);
        fixture.AddUser("locked-admin", admin: true, locked: true);
        if (delete)
            await Assert.ThrowsAsync<AdminOperationException>(() => fixture.Admin.DeleteUserAsync("alice"));
        else
            await Assert.ThrowsAsync<AdminOperationException>(() => fixture.Admin.UpdateUserAsync("alice", new UpdateUserRequest(null, null, ["USER"], [])));
        Assert.Equal(0, user.LogicalDelFlag);
        Assert.DoesNotContain("save", fixture.UnitOfWork.Events);
        Assert.DoesNotContain("commit", fixture.UnitOfWork.Events);
        Assert.Equal(IsolationLevel.Serializable, fixture.UnitOfWork.Isolation);
    }

    [Fact]
    public async Task OneOfTwoActiveAdminsCanBeSoftDeleted()
    {
        using var fixture = new AuthTestFixture();
        var user = fixture.AddUser(admin: true);
        fixture.AddUser("other-admin", admin: true);
        Assert.True(await fixture.Admin.DeleteUserAsync("alice"));
        Assert.Equal(1, user.LogicalDelFlag);
        Assert.False(await fixture.Access.IsActiveAdminAsync("alice"));
        Assert.Null(await fixture.Admin.GetUserAsync("alice"));
    }

    [Fact]
    public async Task InvalidRoleDoesNotSaveOrCommitUserChanges()
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser();
        await Assert.ThrowsAsync<AdminOperationException>(() => fixture.Admin.UpdateUserAsync("alice", new UpdateUserRequest(null, null, ["UNKNOWN"], [])));
        Assert.DoesNotContain("save", fixture.UnitOfWork.Events);
        Assert.DoesNotContain("commit", fixture.UnitOfWork.Events);
    }

    [Fact]
    public async Task CreateNormalizesAccessAndRejectsDuplicateUser()
    {
        using var fixture = new AuthTestFixture();
        var request = new CreateUserRequest(" new-user ", " Nguyen ", " An ", [" USER ", "USER"], ["C000000005"]);
        var result = await fixture.Admin.CreateUserAsync(request);
        Assert.NotNull(result);
        Assert.Equal("new-user", result.UserId);
        Assert.Equal(["USER"], result.Roles);
        Assert.Equal(["C000000005"], result.DirectCategoryIds);
        Assert.Null(await fixture.Admin.CreateUserAsync(request));
    }

    [Fact]
    public async Task PasswordResetUnlocksUserAndClearsPasswordHistory()
    {
        using var fixture = new AuthTestFixture();
        var user = fixture.AddUser(locked: true);
        user.PastPassword = "previous";
        user.PasswordMissCount = 3;
        Assert.True(await fixture.Admin.ResetPasswordAsync("alice"));
        Assert.Equal(0, user.UserLockFlag);
        Assert.Equal(0, user.PasswordMissCount);
        Assert.Null(user.PastPassword);
        Assert.Equal(PasswordVerificationResult.Success, fixture.Hasher.VerifyHashedPassword(user, user.Password!, "123456"));
    }
}
