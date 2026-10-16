namespace OjitexSystem_Backend.Dtos.Factory;

public sealed record ConvertingOnlineDto(
    DateOnly ReportDate, DateTime RefreshedAt, IReadOnlyList<ConvertingOnlineItemDto> Items);

public sealed record ConvertingOnlineItemDto(
    decimal? Shift, decimal? LotNo, decimal? OrderNo, decimal? CustomerCode, string? CustomerName,
    decimal? ProductCode, string? ProductName, decimal? PlanQty, decimal? InputQty,
    decimal? OutputQty, decimal? SurplusQty, decimal? LossQty, decimal ShortestQty,
    string? StartTime, string? FinishTime, decimal? DrivingMinutes, decimal? SetMinutes,
    decimal? StopMinutes, decimal? RestMinutes);
