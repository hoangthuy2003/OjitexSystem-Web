namespace OjitexSystem_Backend.Dtos.Factory;

public sealed record ConvertingHistoryDto(
    DateOnly? ReportDate, DateTime RefreshedAt, IReadOnlyList<ConvertingHistoryItemDto> Items);

public sealed record ConvertingHistoryItemDto(
    DateOnly? Date, decimal? Shift, string? MachineName, decimal? LotNo, decimal? OrderNo,
    decimal? CustomerCode, string? CustomerName, decimal? ProductCode, string? ProductName,
    decimal? PlanQty, decimal? InputQty, decimal? OutputQty, decimal? SurplusQty, decimal? LossQty,
    decimal ShortestQty, string? StartTime, string? FinishTime, decimal? DrivingMinutes,
    decimal? SetMinutes, decimal? StopMinutes, decimal? RestMinutes);
