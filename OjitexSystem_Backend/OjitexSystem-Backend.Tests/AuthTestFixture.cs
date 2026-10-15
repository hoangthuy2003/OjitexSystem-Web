using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Repositories.Admin;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Admin;
using OjitexSystem_Backend.Services.Auth;

namespace OjitexSystem_Backend.Tests;

internal sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => now;
}

internal sealed class AuthTestFixture : IDisposable
{
    public AuthContext Context { get; } = new(new DbContextOptionsBuilder<AuthContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    public TimeProvider Clock { get; } = new FixedTimeProvider(new DateTimeOffset(2026, 1, 31, 18, 0, 0, TimeSpan.Zero));
    public PasswordHasher<IeUser> Hasher { get; } = new(Options.Create(new PasswordHasherOptions { IterationCount = 1000 }));
    public RecordingUnitOfWork UnitOfWork { get; }
    public UserRepository Users { get; }
    public AccessRepository Access { get; }
    public AdminUserRepository AdminUsers { get; }
    public AdminDashboardService Admin { get; }
    public AuthenticationService Authentication { get; }

    public AuthTestFixture()
    {
        UnitOfWork = new RecordingUnitOfWork(Context);
        Users = new UserRepository(Context);
        Access = new AccessRepository(Context);
        AdminUsers = new AdminUserRepository(Context);
        Admin = new AdminDashboardService(AdminUsers, Users, Access, UnitOfWork, Hasher, Clock);
        Authentication = new AuthenticationService(Users, Access, UnitOfWork, Hasher,
            new JwtTokenService("tests", "tests", new byte[32], Clock), Clock);
        Context.IeRoles.AddRange(new IeRole { Role = "ADMIN" }, new IeRole { Role = "USER" });
        Context.IeCategories.AddRange(
            new IeCategory { CategoryId = "C000000005", CategoryName = "LOGISTICS" },
            new IeCategory { CategoryId = "C000000006", CategoryName = "REPORT" });
        Context.SaveChanges();
    }

    public IeUser AddUser(string userId = "alice", bool admin = false, bool locked = false, bool deleted = false)
    {
        var user = new IeUser
        {
            UserId = userId, UserFirstName = "Alice", UserLockFlag = locked ? 1 : 0, LogicalDelFlag = deleted ? 1 : 0
        };
        user.Password = Hasher.HashPassword(user, "original-password");
        Context.IeUsers.Add(user);
        Context.IeUserRoles.Add(new IeUserRole { UserId = userId, Role = admin ? "ADMIN" : "USER" });
        Context.SaveChanges();
        return user;
    }

    public void AddGrant(string userId, string role, string categoryId, bool inherited)
    {
        Context.IeCategoryAuths.Add(new IeCategoryAuth
        {
            UserId = userId, Role = role, CategoryId = categoryId, AuthType = inherited ? 1 : 0, ExecutableType = 1
        });
        Context.SaveChanges();
    }

    public void Dispose() => Context.Dispose();
}

// InMemory cannot exercise SQL transactions. This records the service's transaction
// boundary; transaction isolation on SQL Server requires a separate integration run.
internal sealed class RecordingUnitOfWork(AuthContext context) : IAuthUnitOfWork
{
    public List<string> Events { get; } = [];
    public IsolationLevel? Isolation { get; private set; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Events.Add("save");
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<IAuthTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
    {
        Isolation = isolationLevel;
        Events.Add("begin");
        return Task.FromResult<IAuthTransaction>(new Transaction(Events));
    }

    private sealed class Transaction(List<string> events) : IAuthTransaction
    {
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            events.Add("commit");
            return Task.CompletedTask;
        }
        public ValueTask DisposeAsync()
        {
            events.Add("dispose");
            return ValueTask.CompletedTask;
        }
    }
}
