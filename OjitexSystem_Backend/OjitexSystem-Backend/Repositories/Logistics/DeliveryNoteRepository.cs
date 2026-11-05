using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public class DeliveryNoteRepository(ProductionContext context) : IDeliveryNoteRepository
{
    public async Task<DeliveryNoteHeaderDto?> FindHeaderAsync(string deliverySlipPrefix, CancellationToken cancellationToken = default)
    {
        // Prefix search like the legacy "Like 'ds%'"; an exact slip number wins over longer slips.
        var headers = await context.TLogiDeliveryNotes.AsNoTracking()
            .Where(note => note.DnDeliverySlip.Trim().StartsWith(deliverySlipPrefix))
            .Select(note => new DeliveryNoteHeaderDto
            {
                Key = note.DnKey, DeliverySlip = note.DnDeliverySlip.Trim(), CustomerCode = note.DnCusCd,
                CustomerName = note.DnCusNa, TransportCompany = note.DnTransportCompany,
                TruckNumber = note.DnTruckNumber, CarType = note.DnCarType, DeliveryDate = note.DnDateDelivery,
                State = note.DnState, SupplierCode = note.DnSupCd, Note = note.DnNote,
                DeliveryAddress = note.DnDeliveryAddress, GoodsType = note.DnGoodsType,
                DateChange = note.DnDateChange, DateInput = note.DnDateInput, UserId = note.DnUserId
            }).OrderBy(note => note.DeliverySlip.Length).ThenBy(note => note.DeliverySlip).ThenBy(note => note.Key)
            .Take(1).ToListAsync(cancellationToken);
        return headers.FirstOrDefault();
    }

    public async Task<IReadOnlyList<DeliveryNoteDetailDto>> GetDetailsAsync(string deliverySlip, CancellationToken cancellationToken = default) =>
        await context.TLogiDeliveryNoteDetails.AsNoTracking()
            .Where(detail => detail.DndDeliverySlip != null && detail.DndDeliverySlip.Trim() == deliverySlip)
            .OrderBy(detail => detail.DndLineNo).ThenBy(detail => detail.DndKey)
            .Select(detail => new DeliveryNoteDetailDto
            {
                Key = detail.DndKey, DeliverySlip = detail.DndDeliverySlip, LineNo = detail.DndLineNo,
                ReceiveNo = detail.DndReceiveNo, ProductCode = detail.DndProCd, ProductName = detail.DndProName,
                Quantity = detail.DndQty, CustomerPo = detail.DndCsPo, Remark = detail.DndRemark,
                OtherNote = detail.DndOtherNote
            }).ToListAsync(cancellationToken);
}
