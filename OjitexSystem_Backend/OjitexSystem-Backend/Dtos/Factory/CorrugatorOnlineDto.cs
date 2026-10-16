namespace OjitexSystem_Backend.Dtos.Factory;

public sealed record CorrugatorOnlineDto(
    DateOnly ReportDate, DateTime RefreshedAt, IReadOnlyList<CorrugatorOnlineItemDto> Items);

public sealed record CorrugatorOnlineItemDto(
    DateOnly? PlanDate, decimal? OrderNo, decimal? TargetCut, decimal? QualityItem,
    decimal? PaperWidth, decimal? CorruLength, decimal? SheetWidth, decimal? CorruUp,
    string? Flute, string? Glue, string? Bm, string? Bl, string? Am, string? Al,
    decimal? DriveMinutes, decimal? StopMinutes, decimal? AverageSpeed,
    string? EndTime, DateOnly? CorrugatedDate, DateTime? DateInput);
