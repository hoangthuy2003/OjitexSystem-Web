using OjitexSystem_Backend.Common;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Repositories.Auth;
using OjitexSystem_Backend.Repositories.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public class StockReportService(IStockReportRepository repository, IUserRepository users, TimeProvider timeProvider) : IStockReportService
{
    private (DateOnly From, DateOnly To) GetPeriod(StockReportKind kind, DateOnly? reportDate = null)
    {
        var today = DateOnly.FromDateTime(timeProvider.GetBusinessNow());
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

    public async Task<StockReportDto> GetStockAsync(StockReportKind kind, decimal? productCode = null, CancellationToken cancellationToken = default)
    {
        var report = new StockReportDto { Items = await repository.GetStockAsync(kind, productCode, cancellationToken) };
        if (kind != StockReportKind.Monthly)
        {
            var period = GetPeriod(kind);
            report.WorkingDayFrom = period.From;
            report.WorkingDayTo = period.To;
        }
        return report;
    }

    public async Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly? reportDate = null, CancellationToken cancellationToken = default)
    {
        var period = GetPeriod(kind, reportDate);
        var history = await repository.GetHistoryAsync(kind, productCode, period.From, period.To, cancellationToken);
        var userIds = history.Items.Where(item => !string.IsNullOrWhiteSpace(item.UserId))
            .Select(item => item.UserId!.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        var firstNames = await users.GetFirstNamesAsync(userIds, cancellationToken);
        foreach (var item in history.Items)
            item.UserFirstName = firstNames.GetValueOrDefault(item.UserId?.Trim() ?? "");
        return history;
    }
}
