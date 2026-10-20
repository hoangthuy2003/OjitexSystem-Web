namespace OjitexSystem_Backend.Repositories.Factory;

public sealed record ConvertingOnlineRow
{
    public string? Date { get; init; }
    public string? MachineName { get; init; }
    public decimal? Shift { get; init; }
    public decimal? LotNo { get; init; }
    public decimal? OrderNo { get; init; }
    public decimal? CustomerCode { get; init; }
    public string? CustomerName { get; init; }
    public decimal? ProductCode { get; init; }
    public string? ProductName { get; init; }
    public decimal? PlanQty { get; init; }
    public decimal? InputQty { get; init; }
    public decimal? OutputQty { get; init; }
    public decimal? SurplusQty { get; init; }
    public decimal? LossQty { get; init; }
    public string? StartTime { get; init; }
    public string? FinishTime { get; init; }
    public decimal? DrivingSeconds { get; init; }
    public decimal? SetSeconds { get; init; }
    public decimal? StopSeconds { get; init; }
    public decimal? RestSeconds { get; init; }
}
