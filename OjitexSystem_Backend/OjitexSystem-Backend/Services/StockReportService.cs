using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Repositories;

namespace OjitexSystem_Backend.Services;

public class StockReportService(IStockReportRepository repository, TimeProvider timeProvider) : IStockReportService
{
    private (DateOnly From, DateOnly To) GetPeriod(StockReportKind kind, DateOnly? reportDate = null)
    {
        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().ToOffset(TimeSpan.FromHours(7)).DateTime);
        var currentMonth = new DateOnly(today.Year, today.Month, 1);
        var from = kind switch
        {
            StockReportKind.Current => currentMonth,
            StockReportKind.Last => currentMonth.AddMonths(-1),
            StockReportKind.Monthly when reportDate.HasValue => new DateOnly(reportDate.Value.Year, reportDate.Value.Month, 1),
            StockReportKind.Monthly => throw new ArgumentException("Report date is required for monthly history.", nameof(reportDate)),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
        var to = kind == StockReportKind.Current
            ? today
            : new DateOnly(from.Year, from.Month, DateTime.DaysInMonth(from.Year, from.Month));
        return (from, to);
    }

    public async Task<StockReportDto> GetStockAsync(StockReportKind kind, decimal? productCode = null)
    {
        var report = new StockReportDto { Items = await repository.GetStockAsync(kind, productCode) };
        if (kind != StockReportKind.Monthly)
        {
            var period = GetPeriod(kind);
            report.WorkingDayFrom = period.From;
            report.WorkingDayTo = period.To;
        }
        return report;
    }

    public Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly? reportDate = null)
    {
        var period = GetPeriod(kind, reportDate);
        return repository.GetHistoryAsync(kind, productCode, period.From, period.To);
    }
}
