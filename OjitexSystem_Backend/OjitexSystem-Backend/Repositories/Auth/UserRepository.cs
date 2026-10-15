using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Repositories.Auth;

public sealed class UserRepository(AuthContext context) : IUserRepository
{
    public Task<IeUser?> FindActiveAsync(string userId, CancellationToken cancellationToken = default) =>
        context.IeUsers.SingleOrDefaultAsync(
            user => user.UserId == userId && user.LogicalDelFlag == 0, cancellationToken);

    public Task<bool> ExistsAsync(string userId, CancellationToken cancellationToken = default) =>
        context.IeUsers.AnyAsync(user => user.UserId == userId, cancellationToken);

    public async Task<IReadOnlyList<IeUser>> GetActiveAsync(CancellationToken cancellationToken = default) =>
        await context.IeUsers.Where(user => user.LogicalDelFlag == 0).ToListAsync(cancellationToken);

    public async Task<IReadOnlyDictionary<string, string?>> GetFirstNamesAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0) return new Dictionary<string, string?>();

        var users = await context.IeUsers.AsNoTracking()
            .Where(user => userIds.Contains(user.UserId.Trim()))
            .Select(user => new { user.UserId, user.UserFirstName })
            .ToListAsync(cancellationToken);
        var firstNames = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var user in users)
            firstNames[user.UserId.Trim()] = user.UserFirstName?.Trim();
        return firstNames;
    }
}
