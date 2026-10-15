namespace OjitexSystem_Backend.Dtos.Logistics;

public class StockReportDto
{
    public DateOnly? WorkingDayFrom { get; set; }
    public DateOnly? WorkingDayTo { get; set; }
    public IReadOnlyList<StockReportItemDto> Items { get; set; } = [];
}

public class StockReportItemDto
{
    public int? Id { get; set; }
    public DateOnly? ReportDate { get; set; }
    public decimal? ProductCode { get; set; }
    public decimal? OpeningStock { get; set; }
    public decimal? WarehouseNg { get; set; }
    public decimal? Disposal { get; set; }
    public decimal? Repair { get; set; }
    public decimal? Production { get; set; }
    public decimal? Delivery { get; set; }
    public decimal? ClosingStock { get; set; }
}

public class StockReportHistoryDto
{
    public decimal ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? CustomerName { get; set; }
    public DateOnly WorkingDayFrom { get; set; }
    public DateOnly WorkingDayTo { get; set; }
    public IReadOnlyList<StockReportHistoryItemDto> Items { get; set; } = [];
}

public class StockReportHistoryItemDto
{
    public int Id { get; set; }
    public int? ReceiveNo { get; set; }
    public decimal? ProductCode { get; set; }
    public string? Section { get; set; }
    public decimal? InputStock { get; set; }
    public decimal? OutputStock { get; set; }
    public decimal? CurrentStock { get; set; }
    public string? Remark { get; set; }
    public DateOnly? WorkDate { get; set; }
    public DateTime? DateInput { get; set; }
    public string? UserId { get; set; }
    public string? UserFirstName { get; set; }
}
