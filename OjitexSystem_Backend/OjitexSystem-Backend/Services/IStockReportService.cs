using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Services;

public interface IStockReportService
{
    Task<StockReportDto> GetStockAsync(StockReportKind kind, decimal? productCode = null);
    Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly? reportDate = null);
}
