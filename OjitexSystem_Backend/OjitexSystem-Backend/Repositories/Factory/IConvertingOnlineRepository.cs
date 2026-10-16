namespace OjitexSystem_Backend.Repositories.Factory;

public interface IConvertingOnlineRepository
{
    Task<IReadOnlyList<ConvertingOnlineRow>> GetTodayAsync(
        DateOnly date, string machineName, CancellationToken cancellationToken = default);
}
