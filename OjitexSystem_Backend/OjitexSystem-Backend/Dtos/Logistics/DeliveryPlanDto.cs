namespace OjitexSystem_Backend.Dtos.Logistics;

public class DeliveryPlanSearchResultDto
{
    public IReadOnlyList<DeliveryPlanItemDto> Items { get; set; } = [];
}

public class DeliveryPlanItemDto
{
    public int AcceptanceNo { get; set; }
    public DateOnly? DeliveryDate { get; set; }
    public string? CustomerName { get; set; }
    public decimal? ReceiveNo { get; set; }
    public decimal? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? Stock { get; set; }
    public decimal? CurrentStock { get; set; }
    public decimal? ActualLoading { get; set; }
    public string? PoNo { get; set; }
    public string? Remark { get; set; }
    public decimal? Sqm { get; set; }
    public decimal? TotalSqm { get; set; }
    public string? Flute { get; set; }
    public string? LastProcess { get; set; }
    public string? AcceptanceNote { get; set; }
    public DateTime? DateInput { get; set; }
    public string? UserFirstName { get; set; }
    public DateTime? DateEdit { get; set; }
    public string? UserEditFirstName { get; set; }
}
