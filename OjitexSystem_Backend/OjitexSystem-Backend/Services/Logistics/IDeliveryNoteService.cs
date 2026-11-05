using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Services.Logistics;

public interface IDeliveryNoteService
{
    Task<DeliveryNoteDto?> SearchAsync(string deliverySlip, CancellationToken cancellationToken = default);
}
