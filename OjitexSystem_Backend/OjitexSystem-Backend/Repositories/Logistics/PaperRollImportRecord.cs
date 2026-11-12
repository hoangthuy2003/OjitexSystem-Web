namespace OjitexSystem_Backend.Repositories.Logistics;

public sealed record PaperRollImportRecord(
    string SerialNo,
    string PaperLabel,
    string? GradeName,
    decimal? Grammage,
    decimal? PaperWidth,
    decimal? ImportKg,
    string? PaperType,
    string? SupplierName,
    string? ContainerNo,
    DateTime? DateReceived,
    DateTime? DateInput,
    DateTime? DateEdit,
    string? UserId,
    string? Remark);
