using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public interface IGoodsReceiptRepository
{
    Task<IReadOnlyList<GoodsReceiptItemDto>> SearchAsync(
        DateOnly? dateReceipt, string? receiveNo, string? productCode = null,
        CancellationToken cancellationToken = default);
}
