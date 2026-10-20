using System.Globalization;
using OjitexSystem_Backend.Common;
using OjitexSystem_Backend.Dtos.Factory;
using OjitexSystem_Backend.Repositories.Factory;

namespace OjitexSystem_Backend.Services.Factory;

public sealed class ConvertingOnlineService(IConvertingOnlineRepository repository, TimeProvider clock)
    : IConvertingOnlineService
{
    public async Task<ConvertingOnlineDto> GetTodayAsync(string machineName, CancellationToken cancellationToken = default)
    {
        if (machineName is not ("FDC" or "EVOL" or "FG" or "FAP" or "FPS"))
        {
            throw new ArgumentOutOfRangeException(nameof(machineName));
        }

        var date = DateOnly.FromDateTime(clock.GetBusinessNow());
        var rows = await repository.GetTodayAsync(date, machineName, cancellationToken);
        return new(date, clock.GetBusinessNow(), rows.Select(MapItem).ToArray());
    }

    public async Task<ConvertingHistoryDto> GetHistoryAsync(
        DateOnly? date, string? process, string? productCode, CancellationToken cancellationToken = default)
    {
        process = string.IsNullOrWhiteSpace(process) ? null : process.Trim().ToUpperInvariant();
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();
        var rows = await repository.GetHistoryAsync(date, process, productCode, cancellationToken);
        return new(date, clock.GetBusinessNow(), rows.Select(MapHistoryItem).ToArray());
    }

    private static ConvertingHistoryItemDto MapHistoryItem(ConvertingOnlineRow row)
    {
        var item = MapItem(row);
        var date = DateOnly.TryParseExact(row.Date?.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var parsed) ? parsed : (DateOnly?)null;
        return new(date, item.Shift, row.MachineName?.Trim(' '), item.LotNo, item.OrderNo,
            item.CustomerCode, item.CustomerName, item.ProductCode, item.ProductName,
            item.PlanQty, item.InputQty, item.OutputQty, item.SurplusQty, item.LossQty, item.ShortestQty,
            item.StartTime, item.FinishTime, item.DrivingMinutes, item.SetMinutes, item.StopMinutes, item.RestMinutes);
    }

    private static ConvertingOnlineItemDto MapItem(ConvertingOnlineRow row) => new(
        row.Shift, row.LotNo, row.OrderNo, row.CustomerCode, row.CustomerName?.Trim(' '),
        row.ProductCode, row.ProductName?.Trim(' '), row.PlanQty, row.InputQty, row.OutputQty,
        row.SurplusQty, row.LossQty,
        row.PlanQty is decimal planQty && row.OutputQty is decimal outputQty && planQty > outputQty
            ? planQty - outputQty : 0m,
        FormatTime(row.StartTime), FormatTime(row.FinishTime), ToMinutes(row.DrivingSeconds),
        ToMinutes(row.SetSeconds), ToMinutes(row.StopSeconds), ToMinutes(row.RestSeconds));

    private static decimal? ToMinutes(decimal? seconds) => seconds.HasValue
        ? decimal.Round(seconds.Value / 60m, 1, MidpointRounding.AwayFromZero) : null;

    private static string? FormatTime(string? value)
    {
        value = value?.Trim(' ');
        // Source time is eight characters; SQL extracts HHmm from positions 3 through 6.
        return value is { Length: 8 }
            && TimeOnly.TryParseExact(value[2..6], "HHmm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var time) ? time.ToString("HH:mm", CultureInfo.InvariantCulture) : null;
    }
}
