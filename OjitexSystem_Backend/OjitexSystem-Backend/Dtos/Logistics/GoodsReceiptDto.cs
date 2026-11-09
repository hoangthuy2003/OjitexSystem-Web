namespace OjitexSystem_Backend.Dtos.Logistics;

public class GoodsReceiptSearchResultDto
{
    public IReadOnlyList<GoodsReceiptItemDto> Items { get; set; } = [];
}

public class GoodsReceiptItemDto
{
    public int Key { get; set; }
    public DateOnly? DateReceipt { get; set; }
    public decimal? ReceiveNo { get; set; }
    public decimal? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public decimal? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? Quantity { get; set; }
    public string? ProductionSection { get; set; }
    public string? Note { get; set; }
    public DateTime? DateInput { get; set; }
    public string? UserFirstName { get; set; }
    public DateTime? DateChanged { get; set; }
    public string? UserChangedFirstName { get; set; }
}
