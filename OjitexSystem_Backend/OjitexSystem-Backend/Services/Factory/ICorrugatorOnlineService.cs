using OjitexSystem_Backend.Dtos.Factory;

namespace OjitexSystem_Backend.Services.Factory;

public interface ICorrugatorOnlineService
{
    Task<CorrugatorOnlineDto> GetTodayAsync(CancellationToken cancellationToken = default);
}
