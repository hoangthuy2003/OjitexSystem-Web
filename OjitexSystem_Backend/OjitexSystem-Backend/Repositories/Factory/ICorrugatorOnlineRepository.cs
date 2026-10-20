using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Repositories.Factory;

public interface ICorrugatorOnlineRepository
{
    Task<IReadOnlyList<TCorrugatorOnlineReport>> GetByDateAsync(
        DateOnly date, CancellationToken cancellationToken = default);
}
