using System.Globalization;
using OjitexSystem_Backend.Common;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos.Factory;
using OjitexSystem_Backend.Repositories.Factory;

namespace OjitexSystem_Backend.Services.Factory;

public sealed class CorrugatorOnlineService(ICorrugatorOnlineRepository repository, TimeProvider clock)
    : ICorrugatorOnlineService
{
    public Task<CorrugatorOnlineDto> GetTodayAsync(CancellationToken cancellationToken = default) =>
        GetHistoryAsync(DateOnly.FromDateTime(clock.GetBusinessNow()), cancellationToken);

    public async Task<CorrugatorOnlineDto> GetHistoryAsync(
        DateOnly date, CancellationToken cancellationToken = default)
    {
        var rows = await repository.GetByDateAsync(date, cancellationToken);
        return new(date, clock.GetBusinessNow(), rows.Select(MapItem).ToArray());
    }

    private static CorrugatorOnlineItemDto MapItem(TCorrugatorOnlineReport row)
    {
        var driveMinutes = ToMinutes(row.CoorNo38DriveTime);
        decimal? speed = 0;
        if (row.CoorNo34QualityItem > 0 && row.CoorNo3CorruLength > 0 && row.CoorNo38DriveTime > 0
            && driveMinutes > 0)
        {
            speed = decimal.Floor(row.CoorNo34QualityItem.Value * row.CoorNo3CorruLength.Value
                / driveMinutes.Value / 1000m);
        }

        var timestamp = row.CoorNo40CorrugatedDatetime?.Trim();
        var endTime = timestamp is { Length: 10 }
            && TimeOnly.TryParseExact(timestamp[6..], "HHmm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var time) ? time.ToString("HH:mm") : null;
        return new(ParseDate(row.CoorNo41PlanDate), row.CoorNo1OrderNo, row.CoorNo4TargetCut,
            row.CoorNo34QualityItem, row.CoorNo9PaperWidth, row.CoorNo3CorruLength,
            row.CoorNo11SheetWidth, row.CoorNo10Sl1, row.CoorNo5Flute?.Trim(' '),
            Paper(row.CoorNo32Dfl), Paper(row.CoorNo31Sf2m), Paper(row.CoorNo30Sf2l),
            Paper(row.CoorNo29Sf1m), Paper(row.CoorNo28Sf1l), driveMinutes,
            ToMinutes(row.CoorNo39StopTime), speed, endTime,
            ParseDate(timestamp is { Length: >= 6 } ? timestamp[..6] : null), row.CoorNo42DateInput);
    }

    private static decimal? ToMinutes(decimal? seconds) => seconds.HasValue
        ? decimal.Round(seconds.Value / 60m, 1, MidpointRounding.AwayFromZero) : null;

    private static string? Paper(string? value)
    {
        if (value is null) return null;
        return value[..Math.Min(3, value.Length)].Trim(' ').Length == 0 ? "" : value.Trim(' ');
    }

    private static DateOnly? ParseDate(string? value)
    {
        value = value?.Trim(' ');
        if (value is not { Length: 6 }) return null;
        // Match SQL Server's default two-digit year cutoff (2049).
        var century = string.CompareOrdinal(value[..2], "49") <= 0 ? "20" : "19";
        return DateOnly.TryParseExact(century + value, "yyyyMMdd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var date) ? date : null;
    }
}
