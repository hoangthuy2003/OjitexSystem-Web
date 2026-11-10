using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public class DeliveryPlanRepository(ProductionContext context) : IDeliveryPlanRepository
{
    public async Task<IReadOnlyList<DeliveryPlanItemDto>> SearchAsync(
        DateOnly? deliveryDate, string? receiveNo,
        CancellationToken cancellationToken = default)
    {
        var query =
            from plan in context.TLogiDeliveryPlans.AsNoTracking()
            join stock in context.TCurrentStocks.AsNoTracking()
                on plan.DpProCd equals (decimal?)stock.CstProCd into stocks
            from stock in stocks.DefaultIfEmpty()
            select new { plan, CurrentStock = stock != null ? stock.CstStock : null };

        if (deliveryDate.HasValue)
        {
            query = query.Where(row => row.plan.DpDateDelivery == deliveryDate.Value);
        }

        if (!string.IsNullOrEmpty(receiveNo))
        {
            query = query.Where(row =>
                row.plan.DpReceiveNo != null &&
                row.plan.DpReceiveNo.Value.ToString().StartsWith(receiveNo));
        }

        return await query
            .OrderBy(row => row.plan.DpProCd)
            .ThenBy(row => row.plan.DpId)
            .Select(row => new DeliveryPlanItemDto
            {
                AcceptanceNo = row.plan.DpId,
                DeliveryDate = row.plan.DpDateDelivery,
                CustomerName = row.plan.DpCusNa,
                ReceiveNo = row.plan.DpReceiveNo,
                ProductCode = row.plan.DpProCd,
                ProductName = row.plan.DpProName,
                Quantity = row.plan.DpQty,
                Stock = row.plan.DpStock,
                CurrentStock = row.CurrentStock,
                ActualLoading = row.plan.DpAutualQty,
                PoNo = row.plan.DpPoNo,
                Remark = row.plan.DpRemark,
                Sqm = row.plan.DpSqm,
                TotalSqm = row.plan.DpTotalSqm,
                Flute = row.plan.DpFlute,
                LastProcess = row.plan.DpLastProcess,
                AcceptanceNote = row.plan.DpAcceptanceNote,
                DateInput = row.plan.DpDateInput,
                DateEdit = row.plan.DpDateChanged,
                UserFirstName = row.plan.DpUserId,
                UserEditFirstName = row.plan.DpUserIdChanged,
            })
            .ToListAsync(cancellationToken);
    }
}
