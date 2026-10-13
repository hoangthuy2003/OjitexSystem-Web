using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Repositories;

namespace OjitexSystem_Backend.Services;

public class StockReportService(IStockReportRepository repository, TimeProvider timeProvider) : IStockReportService
{
    private DateOnly LastMonthStart()
    {
        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().ToOffset(TimeSpan.FromHours(7)).DateTime);
        return new DateOnly(today.Year, today.Month, 1).AddMonths(-1);
    }

    public async Task<StockReportDto> GetLastStockAsync(decimal? productCode = null)
    {
        var from = LastMonthStart();
        return new StockReportDto
        {
            WorkingDayFrom = from, WorkingDayTo = from.AddMonths(1).AddDays(-1),
            Items = await repository.GetLastStockAsync(productCode)
        };
    }

    public async Task<StockReportDto> GetMonthlyStockAsync(decimal? productCode = null) =>
        new() { Items = await repository.GetMonthlyStockAsync(productCode) };

    public Task<StockReportHistoryDto> GetLastHistoryAsync(decimal productCode)
    {
        var from = LastMonthStart();
        return repository.GetHistoryAsync(productCode, from, from.AddMonths(1).AddDays(-1), false);
    }

    public Task<StockReportHistoryDto> GetMonthlyHistoryAsync(decimal productCode, DateOnly reportDate)
    {
        var from = new DateOnly(reportDate.Year, reportDate.Month, 1);
        var to = new DateOnly(from.Year, from.Month, DateTime.DaysInMonth(from.Year, from.Month));
        return repository.GetHistoryAsync(productCode, from, to, true);
    }
}
