using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public interface IStockReportService
{
    Task<StockReportDto> GetStockAsync(StockReportKind kind, decimal? productCode = null, CancellationToken cancellationToken = default);
    Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly? reportDate = null, CancellationToken cancellationToken = default);
}
