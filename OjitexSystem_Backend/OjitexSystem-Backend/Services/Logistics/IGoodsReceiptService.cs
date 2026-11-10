using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public interface IGoodsReceiptService
{
    Task<GoodsReceiptSearchResultDto> SearchAsync(
        DateOnly? dateReceipt, string? receiveNo,
        CancellationToken cancellationToken = default);
    Task<GoodsReceiptInformationDto> GetInformationAsync(
        DateOnly? dateReceipt, string? receiveNo, string? productCode,
        CancellationToken cancellationToken = default);
}
