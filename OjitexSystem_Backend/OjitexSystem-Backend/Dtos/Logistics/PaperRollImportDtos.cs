namespace OjitexSystem_Backend.Dtos.Logistics;

public class PaperRollImportSearchResultDto
{
    public IReadOnlyList<PaperRollImportItemDto> Items { get; set; } = [];
}

public class PaperRollImportItemDto
{
    public string SerialNo { get; set; } = string.Empty;
    public string PaperLabel { get; set; } = string.Empty;
    public string? GradeName { get; set; }
    public decimal? Grammage { get; set; }
    public decimal? PaperWidth { get; set; }
    public decimal? ImportKg { get; set; }
    public string? PaperType { get; set; }
    public string? SupplierName { get; set; }
    public string? ContainerNo { get; set; }
    public DateTime? DateReceived { get; set; }
    public DateTime? DateInput { get; set; }
    public DateTime? DateEdit { get; set; }
    public string? UserInput { get; set; }
    public string? Remark { get; set; }
}
