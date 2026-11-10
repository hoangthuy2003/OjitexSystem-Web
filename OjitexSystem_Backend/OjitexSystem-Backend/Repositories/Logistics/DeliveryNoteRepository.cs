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

    public async Task<IReadOnlyList<DeliveryInformationItemDto>> GetInformationAsync(
        string? deliverySlip, string? customerCode, string? productCode, DateOnly? deliveryDate,
        CancellationToken cancellationToken = default)
    {
        var query =
            from detail in context.TLogiDeliveryNoteDetails.AsNoTracking()
            join note in context.TLogiDeliveryNotes.AsNoTracking()
                on detail.DndDeliverySlip equals note.DnDeliverySlip
            join product in context.TInputProducts.AsNoTracking()
                on detail.DndProCd equals (decimal?)product.ProCd into products
            from product in products.DefaultIfEmpty()
            where note.DnUserId != null && note.DnUserId.Trim() != ""
            select new
            {
                detail,
                note,
                Flute = product != null ? product.ProFlute : null,
                SheetSqm = product != null ? product.ProSheetSqu : null
            };

        if (!string.IsNullOrEmpty(deliverySlip))
        {
            query = query.Where(row => row.detail.DndDeliverySlip != null
                && row.detail.DndDeliverySlip.Trim().StartsWith(deliverySlip));
        }

        if (!string.IsNullOrEmpty(customerCode))
        {
            query = query.Where(row => row.note.DnCusCd.HasValue
                && row.note.DnCusCd.Value.ToString().StartsWith(customerCode));
        }

        if (!string.IsNullOrEmpty(productCode))
        {
            query = query.Where(row => row.detail.DndProCd.HasValue
                && row.detail.DndProCd.Value.ToString().StartsWith(productCode));
        }

        if (deliveryDate.HasValue)
        {
            var date = deliveryDate.Value;
            query = query.Where(row => row.note.DnDateDelivery == date);
        }

        return await query
            .OrderByDescending(row => row.note.DnDateDelivery)
            .ThenByDescending(row => row.note.DnDateInput)
            .ThenBy(row => row.note.DnDeliverySlip)
            .ThenBy(row => row.detail.DndLineNo)
            .ThenBy(row => row.detail.DndKey)
            .Select(row => new DeliveryInformationItemDto
            {
                Key = row.detail.DndKey,
                LineNo = row.detail.DndLineNo,
                DeliverySlip = row.detail.DndDeliverySlip != null ? row.detail.DndDeliverySlip.Trim() : "",
                ReceiveNo = row.detail.DndReceiveNo,
                CustomerCode = row.note.DnCusCd,
                CustomerName = row.note.DnCusNa,
                ProductCode = row.detail.DndProCd,
                ProductName = row.detail.DndProName,
                Quantity = row.detail.DndQty,
                DeliveryDate = row.note.DnDateDelivery,
                CustomerPo = row.detail.DndCsPo,
                Remark = row.detail.DndRemark,
                OtherNote = row.detail.DndOtherNote,
                TruckNumber = row.note.DnTruckNumber,
                CarType = row.note.DnCarType,
                TransportCompany = row.note.DnTransportCompany,
                DateInput = row.note.DnDateInput,
                UserId = row.note.DnUserId,
                State = row.note.DnState,
                Flute = row.Flute,
                SheetSqm = row.SheetSqm,
                TotalSqm = row.detail.DndQty.HasValue && row.SheetSqm.HasValue
                    ? row.detail.DndQty.Value * row.SheetSqm.Value
                    : null
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DeliveryMonthlyItemDto>> GetMonthlyAsync(
        string? deliverySlip, string? customerCode, string? productCode,
        DateOnly? fromDate, DateOnly? toDate,
        CancellationToken cancellationToken = default)
    {
        var query =
            from detail in context.TLogiDeliveryNoteDetails.AsNoTracking()
            join note in context.TLogiDeliveryNotes.AsNoTracking()
                on detail.DndDeliverySlip equals note.DnDeliverySlip
            where note.DnUserId != null && note.DnUserId.Trim() != ""
            select new { detail, note };

        if (!string.IsNullOrEmpty(deliverySlip))
        {
            query = query.Where(row => row.detail.DndDeliverySlip != null
                && row.detail.DndDeliverySlip.Trim().StartsWith(deliverySlip));
        }

        if (!string.IsNullOrEmpty(customerCode))
        {
            query = query.Where(row => row.note.DnCusCd.HasValue
                && row.note.DnCusCd.Value.ToString().StartsWith(customerCode));
        }

        if (!string.IsNullOrEmpty(productCode))
        {
            query = query.Where(row => row.detail.DndProCd.HasValue
                && row.detail.DndProCd.Value.ToString().StartsWith(productCode));
        }

        if (fromDate.HasValue)
        {
            var from = fromDate.Value;
            query = query.Where(row => row.note.DnDateDelivery >= from);
        }

        if (toDate.HasValue)
        {
            var to = toDate.Value;
            query = query.Where(row => row.note.DnDateDelivery <= to);
        }

        return await query
            .OrderByDescending(row => row.note.DnDateDelivery)
            .ThenByDescending(row => row.note.DnDateInput)
            .ThenBy(row => row.note.DnDeliverySlip)
            .ThenBy(row => row.detail.DndLineNo)
            .ThenBy(row => row.detail.DndKey)
            .Select(row => new DeliveryMonthlyItemDto
            {
                Key = row.detail.DndKey,
                Status = row.note.DnStatus,
                LineNo = row.detail.DndLineNo,
                DeliverySlip = row.detail.DndDeliverySlip != null ? row.detail.DndDeliverySlip.Trim() : "",
                ReceiveNo = row.detail.DndReceiveNo,
                SupplierCode = row.note.DnSupCd,
                CustomerCode = row.note.DnCusCd,
                CustomerName = row.note.DnCusNa,
                ProductCode = row.detail.DndProCd,
                ProductName = row.detail.DndProName,
                Quantity = row.detail.DndQty,
                DeliveryDate = row.note.DnDateDelivery,
                CustomerPo = row.detail.DndCsPo,
                Remark = row.detail.DndRemark,
                OtherNote = row.detail.DndOtherNote,
                TruckNumber = row.note.DnTruckNumber,
                CarType = row.note.DnCarType,
                TransportCompany = row.note.DnTransportCompany,
                DateInput = row.note.DnDateInput,
                UserId = row.note.DnUserId,
                State = row.note.DnState
            })
            .ToListAsync(cancellationToken);
    }
}
