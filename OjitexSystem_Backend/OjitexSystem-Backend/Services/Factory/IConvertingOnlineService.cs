using OjitexSystem_Backend.Dtos.Factory;

namespace OjitexSystem_Backend.Services.Factory;

public interface IConvertingOnlineService
{
    Task<ConvertingOnlineDto> GetTodayAsync(string machineName, CancellationToken cancellationToken = default);
    Task<ConvertingHistoryDto> GetHistoryAsync(
        DateOnly? date, string? process, string? productCode, CancellationToken cancellationToken = default);
}
