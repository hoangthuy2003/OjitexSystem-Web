using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Repositories;

public interface IStockReportRepository
{
    Task<IReadOnlyList<StockReportItemDto>> GetStockAsync(StockReportKind kind, decimal? productCode);
    Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly from, DateOnly to);
}
