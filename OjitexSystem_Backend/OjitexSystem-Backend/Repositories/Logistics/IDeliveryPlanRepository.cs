using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public interface IDeliveryPlanRepository
{
    Task<IReadOnlyList<DeliveryPlanItemDto>> SearchAsync(
        DateOnly? deliveryDate, string? receiveNo,
        CancellationToken cancellationToken = default);
}
