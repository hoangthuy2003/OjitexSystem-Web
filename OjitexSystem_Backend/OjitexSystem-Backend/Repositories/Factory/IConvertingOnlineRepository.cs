namespace OjitexSystem_Backend.Repositories.Factory;

public interface IConvertingOnlineRepository
{
    Task<IReadOnlyList<ConvertingOnlineRow>> GetTodayAsync(
        DateOnly date, string machineName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ConvertingOnlineRow>> GetHistoryAsync(
        DateOnly? date, string? process, string? productCode, CancellationToken cancellationToken = default);
}
