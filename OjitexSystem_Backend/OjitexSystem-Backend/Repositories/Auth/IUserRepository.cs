using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Repositories.Auth;

public interface IUserRepository
{
    Task<IeUser?> FindActiveAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<IeUser>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyDictionary<string, string?>> GetFirstNamesAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default);
}
