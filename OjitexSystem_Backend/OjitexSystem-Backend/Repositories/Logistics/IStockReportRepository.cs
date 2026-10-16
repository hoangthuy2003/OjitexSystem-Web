using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public interface IStockReportRepository
{
    Task<IReadOnlyList<StockReportItemDto>> GetStockAsync(StockReportKind kind, decimal? productCode, CancellationToken cancellationToken = default);
    Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
}
