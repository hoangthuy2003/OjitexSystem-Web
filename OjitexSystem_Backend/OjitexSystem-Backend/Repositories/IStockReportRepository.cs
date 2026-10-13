using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Repositories;

public interface IStockReportRepository
{
    Task<IReadOnlyList<StockReportItemDto>> GetLastStockAsync(decimal? productCode);
    Task<IReadOnlyList<StockReportItemDto>> GetMonthlyStockAsync(decimal? productCode);
    Task<StockReportHistoryDto> GetHistoryAsync(decimal productCode, DateOnly from, DateOnly to, bool monthly);
}
