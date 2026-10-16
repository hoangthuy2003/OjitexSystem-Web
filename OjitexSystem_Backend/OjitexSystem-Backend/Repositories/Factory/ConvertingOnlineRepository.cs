using System.Globalization;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Repositories.Factory;

public sealed class ConvertingOnlineRepository(ProductionContext context) : IConvertingOnlineRepository
{
    public async Task<IReadOnlyList<ConvertingOnlineRow>> GetTodayAsync(
        DateOnly date, string machineName, CancellationToken cancellationToken = default)
    {
        var reportDate = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        return await (
            from row in context.TConvertingOnlineReports.AsNoTracking()
            join customer in context.TInputCustomers.AsNoTracking()
                on row.CorNo8CustomerCode equals customer.CusCd
            join product in context.TInputProducts.AsNoTracking()
                on row.CorNo9ProductCode equals product.ProCd
            where row.CorNo1Date == reportDate && row.CorNo43MachineName == machineName
            orderby row.CorNo3Shift, row.CorNo30StartTime, row.CorNo6LotNo,
                row.CorNo7OrderNo, row.CorNo8CustomerCode, row.CorNo9ProductCode,
                row.CorNo31FinishTime, row.CorNo12CaseQty, row.CorNo14FeedQty,
                row.CorNo15FinishQty, row.CorNo16SurplusQty, row.CorNo21LossQtyOwn,
                row.CorNo32DrivingTime, row.CorNo33SetTime, row.CorNo34StopTime, row.CorNo35RestTime
            select new ConvertingOnlineRow
            {
                Shift = row.CorNo3Shift,
                LotNo = row.CorNo6LotNo,
                OrderNo = row.CorNo7OrderNo,
                CustomerCode = row.CorNo8CustomerCode,
                CustomerName = customer.CusName,
                ProductCode = row.CorNo9ProductCode,
                ProductName = product.ProName,
                PlanQty = row.CorNo12CaseQty,
                InputQty = row.CorNo14FeedQty,
                OutputQty = row.CorNo15FinishQty,
                SurplusQty = row.CorNo16SurplusQty,
                LossQty = row.CorNo21LossQtyOwn,
                StartTime = row.CorNo30StartTime,
                FinishTime = row.CorNo31FinishTime,
                DrivingSeconds = row.CorNo32DrivingTime,
                SetSeconds = row.CorNo33SetTime,
                StopSeconds = row.CorNo34StopTime,
                RestSeconds = row.CorNo35RestTime
            }).ToListAsync(cancellationToken);
    }
}
