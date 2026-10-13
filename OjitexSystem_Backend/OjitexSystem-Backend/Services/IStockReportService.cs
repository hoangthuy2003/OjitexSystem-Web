using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Services;

public interface IStockReportService
{
    Task<StockReportDto> GetLastStockAsync(decimal? productCode = null);
    Task<StockReportDto> GetMonthlyStockAsync(decimal? productCode = null);
    Task<StockReportHistoryDto> GetLastHistoryAsync(decimal productCode);
    Task<StockReportHistoryDto> GetMonthlyHistoryAsync(decimal productCode, DateOnly reportDate);
}
