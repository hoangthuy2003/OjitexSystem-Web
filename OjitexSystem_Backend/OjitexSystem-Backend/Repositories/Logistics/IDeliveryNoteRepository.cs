using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public interface IDeliveryNoteRepository
{
    Task<DeliveryNoteHeaderDto?> FindHeaderAsync(string deliverySlipPrefix, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeliveryNoteDetailDto>> GetDetailsAsync(string deliverySlip, CancellationToken cancellationToken = default);
}
