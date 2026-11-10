using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public interface IDeliveryPlanRepository
{
    Task<IReadOnlyList<DeliveryPlanItemDto>> SearchAsync(
        DateOnly? deliveryDate, string? receiveNo,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DeliveryPlanMonthlyItemDto>> GetMonthlyAsync(
        DateOnly? fromDate, DateOnly? toDate, string? productCode,
        CancellationToken cancellationToken = default);
}
