namespace OjitexSystem_Backend.Tests;

public sealed class AccessTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CategoryAccessAcceptsDirectOrInheritedGrants(bool inherited)
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser();
        fixture.AddGrant(inherited ? "*" : "alice", inherited ? "USER" : "*", "C000000005", inherited);
        Assert.True(await fixture.Access.HasCategoryAsync("alice", "C000000005"));
        Assert.False(await fixture.Access.HasCategoryAsync("alice", "C000000006"));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task LockedOrDeletedUsersLoseBothAdminAndCategoryAccess(bool locked, bool deleted)
    {
        using var fixture = new AuthTestFixture();
        fixture.AddUser(admin: true, locked: locked, deleted: deleted);
        fixture.AddGrant("alice", "*", "C000000005", false);
        Assert.False(await fixture.Access.IsActiveAdminAsync("alice"));
        Assert.False(await fixture.Access.HasCategoryAsync("alice", "C000000005"));
    }
}
