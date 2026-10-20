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
        return await QueryRows()
            .Where(row => row.Date == reportDate && row.MachineName == machineName)
            .OrderBy(row => row.Shift).ThenBy(row => row.StartTime).ThenBy(row => row.LotNo)
            .ThenBy(row => row.OrderNo).ThenBy(row => row.CustomerCode).ThenBy(row => row.ProductCode)
            .ThenBy(row => row.FinishTime).ThenBy(row => row.PlanQty).ThenBy(row => row.InputQty)
            .ThenBy(row => row.OutputQty).ThenBy(row => row.SurplusQty).ThenBy(row => row.LossQty)
            .ThenBy(row => row.DrivingSeconds).ThenBy(row => row.SetSeconds)
            .ThenBy(row => row.StopSeconds).ThenBy(row => row.RestSeconds)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ConvertingOnlineRow>> GetHistoryAsync(
        DateOnly? date, string? process, string? productCode, CancellationToken cancellationToken = default)
    {
        var query = QueryRows();
        if (date.HasValue)
        {
            var reportDate = date.Value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            query = query.Where(row => row.Date == reportDate);
        }
        if (!string.IsNullOrEmpty(process))
        {
            query = query.Where(row => row.MachineName != null
                && row.MachineName.Trim().ToUpper().StartsWith(process));
        }
        if (!string.IsNullOrEmpty(productCode))
        {
            // Preserve SQL's textual prefix matching, including a literal leading zero.
            query = query.Where(row => row.ProductCode.HasValue
                && row.ProductCode.Value.ToString().StartsWith(productCode));
        }

        return await query.OrderBy(row => row.StartTime).ThenBy(row => row.Date)
            .ThenBy(row => row.MachineName).ThenBy(row => row.Shift).ThenBy(row => row.LotNo)
            .ThenBy(row => row.OrderNo).ThenBy(row => row.CustomerCode).ThenBy(row => row.ProductCode)
            .ThenBy(row => row.FinishTime).ThenBy(row => row.PlanQty).ThenBy(row => row.InputQty)
            .ThenBy(row => row.OutputQty).ThenBy(row => row.SurplusQty).ThenBy(row => row.LossQty)
            .ThenBy(row => row.DrivingSeconds).ThenBy(row => row.SetSeconds)
            .ThenBy(row => row.StopSeconds).ThenBy(row => row.RestSeconds)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<ConvertingOnlineRow> QueryRows() =>
        from row in context.TConvertingOnlineReports.AsNoTracking()
        join customer in context.TInputCustomers.AsNoTracking()
            on row.CorNo8CustomerCode equals customer.CusCd
        join product in context.TInputProducts.AsNoTracking()
            on row.CorNo9ProductCode equals product.ProCd
        select new ConvertingOnlineRow
        {
            Date = row.CorNo1Date,
            MachineName = row.CorNo43MachineName,
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
        };
}
