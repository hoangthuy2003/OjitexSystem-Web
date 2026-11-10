using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public interface IDeliveryNoteRepository
{
    Task<DeliveryNoteHeaderDto?> FindHeaderAsync(string deliverySlipPrefix, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeliveryNoteDetailDto>> GetDetailsAsync(string deliverySlip, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeliveryInformationItemDto>> GetInformationAsync(
        string? deliverySlip, string? customerCode, string? productCode, DateOnly? deliveryDate,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeliveryMonthlyItemDto>> GetMonthlyAsync(
        string? deliverySlip, string? customerCode, string? productCode,
        DateOnly? fromDate, DateOnly? toDate,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeliveryStatusItemDto>> GetStatusAsync(
        DateOnly? fromDate, DateOnly? toDate,
        CancellationToken cancellationToken = default);
}
