namespace OjitexSystem_Backend.Dtos.Logistics;

public class DeliveryNoteDto
{
    public DeliveryNoteHeaderDto Header { get; set; } = new();
    public IReadOnlyList<DeliveryNoteDetailDto> Details { get; set; } = [];
}

public class DeliveryNoteHeaderDto
{
    public int Key { get; set; }
    public string DeliverySlip { get; set; } = "";
    public decimal? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? TransportCompany { get; set; }
    public string? TruckNumber { get; set; }
    public string? CarType { get; set; }
    public DateOnly? DeliveryDate { get; set; }
    public string? State { get; set; }
    public string? SupplierCode { get; set; }
    public string? Note { get; set; }
    public string? DeliveryAddress { get; set; }
    public string? GoodsType { get; set; }
    public DateTime? DateChange { get; set; }
    public DateTime? DateInput { get; set; }
    public string? UserId { get; set; }
    public string? UserFirstName { get; set; }
}

public class DeliveryNoteDetailDto
{
    public long Key { get; set; }
    public string? DeliverySlip { get; set; }
    public int? LineNo { get; set; }
    public decimal? ReceiveNo { get; set; }
    public decimal? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? Quantity { get; set; }
    public string? CustomerPo { get; set; }
    public string? Remark { get; set; }
    public string? OtherNote { get; set; }
}

public class DeliveryInformationDto
{
    public IReadOnlyList<DeliveryInformationItemDto> Items { get; set; } = [];
}

public class DeliveryInformationItemDto
{
    public long Key { get; set; }
    public int? LineNo { get; set; }
    public string DeliverySlip { get; set; } = "";
    public decimal? ReceiveNo { get; set; }
    public decimal? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public decimal? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? Quantity { get; set; }
    public DateOnly? DeliveryDate { get; set; }
    public string? CustomerPo { get; set; }
    public string? Remark { get; set; }
    public string? OtherNote { get; set; }
    public string? TruckNumber { get; set; }
    public string? CarType { get; set; }
    public string? TransportCompany { get; set; }
    public DateTime? DateInput { get; set; }
    public string? UserId { get; set; }
    public string? UserFirstName { get; set; }
    public string? State { get; set; }
    public string? Flute { get; set; }
    public decimal? SheetSqm { get; set; }
    public decimal? TotalSqm { get; set; }
}

public class DeliveryMonthlyReportDto
{
    public IReadOnlyList<DeliveryMonthlyItemDto> Items { get; set; } = [];
}

public class DeliveryMonthlyItemDto
{
    public long Key { get; set; }
    public string? Status { get; set; }
    public int? LineNo { get; set; }
    public string DeliverySlip { get; set; } = "";
    public decimal? ReceiveNo { get; set; }
    public string? SupplierCode { get; set; }
    public decimal? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public decimal? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? Quantity { get; set; }
    public DateOnly? DeliveryDate { get; set; }
    public string? CustomerPo { get; set; }
    public string? Remark { get; set; }
    public string? OtherNote { get; set; }
    public string? TruckNumber { get; set; }
    public string? CarType { get; set; }
    public string? TransportCompany { get; set; }
    public DateTime? DateInput { get; set; }
    public string? UserId { get; set; }
    public string? UserFirstName { get; set; }
    public string? State { get; set; }
}

public class DeliveryStatusReportDto
{
    public IReadOnlyList<DeliveryStatusItemDto> Items { get; set; } = [];
}

public class DeliveryStatusItemDto
{
    public int Key { get; set; }
    public decimal? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public DateOnly? DeliveryDate { get; set; }
    public string? TransportSupplier { get; set; }
    public string? TruckNumber { get; set; }
    public string DeliverySlip { get; set; } = "";
    public string? Status { get; set; }
    public string? Remark { get; set; }
}

