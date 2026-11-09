using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public class GoodsReceiptRepository(ProductionContext context) : IGoodsReceiptRepository
{
    public async Task<IReadOnlyList<GoodsReceiptItemDto>> SearchAsync(
        DateOnly? dateReceipt, string? receiveNo,
        CancellationToken cancellationToken = default)
    {
        var query =
            from gr in context.TLogiGoodsReceipts.AsNoTracking()
            join customer in context.TInputCustomers.AsNoTracking()
                on gr.GrCusCd equals (decimal?)customer.CusCd into customers
            from customer in customers.DefaultIfEmpty()
            select new { gr, CustomerName = customer != null ? customer.CusName : null };

        if (dateReceipt.HasValue)
        {
            query = query.Where(row => row.gr.GrDateReceipt == dateReceipt.Value);
        }

        if (!string.IsNullOrEmpty(receiveNo))
        {
            query = query.Where(row =>
                row.gr.GrReceiveNo != null &&
                row.gr.GrReceiveNo.Value.ToString().StartsWith(receiveNo));
        }

        return await query
            .OrderByDescending(row => row.gr.GrDateInput)
            .Select(row => new GoodsReceiptItemDto
            {
                Key = row.gr.GrKey,
                DateReceipt = row.gr.GrDateReceipt,
                ReceiveNo = row.gr.GrReceiveNo,
                CustomerCode = row.gr.GrCusCd,
                CustomerName = row.CustomerName,
                ProductCode = row.gr.GrProCd,
                ProductName = row.gr.GrProName,
                Quantity = row.gr.GrQty,
                ProductionSection = row.gr.GrProductionSection,
                Note = row.gr.GrNote,
                DateInput = row.gr.GrDateInput,
                DateChanged = row.gr.GrDateChanged,
                // UserId and UserChangedId resolved via IUserRepository in the service layer
                UserFirstName = row.gr.GrUserId,
                UserChangedFirstName = row.gr.GrUserChanged,
            })
            .ToListAsync(cancellationToken);
    }
}
