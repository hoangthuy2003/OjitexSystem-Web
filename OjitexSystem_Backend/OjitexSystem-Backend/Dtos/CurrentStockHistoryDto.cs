namespace OjitexSystem_Backend.Dtos;

public class CurrentStockHistoryDto
{
    public decimal ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? CustomerName { get; set; }
    public DateOnly? WorkingDayFrom { get; set; }
    public DateOnly? WorkingDayTo { get; set; }
    public IReadOnlyList<CurrentStockHistoryItemDto> Items { get; set; } = [];
}

public class CurrentStockHistoryItemDto
{
    public int CsthId { get; set; }
    public int? CsthReceiveNo { get; set; }
    public decimal? CsthProCd { get; set; }
    public string? CsthSection { get; set; }
    public decimal? CsthInputStock { get; set; }
    public decimal? CsthOutputStock { get; set; }
    public decimal? CsthCurrentStock { get; set; }
    public string? CsthRemark { get; set; }
    public DateOnly? CsthWorkDate { get; set; }
    public DateTime? CsthDateInput { get; set; }
    public string? CsthUserId { get; set; }
    public string? UserFirstName { get; set; }
}
