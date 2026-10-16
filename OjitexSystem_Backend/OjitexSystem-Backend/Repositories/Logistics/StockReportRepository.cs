using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos.Logistics;

namespace OjitexSystem_Backend.Repositories.Logistics;

public class StockReportRepository(ProductionContext context) : IStockReportRepository
{
    public Task<IReadOnlyList<StockReportItemDto>> GetStockAsync(StockReportKind kind, decimal? productCode, CancellationToken cancellationToken = default) => kind switch
    {
        StockReportKind.Current => GetCurrentStockAsync(productCode, cancellationToken),
        StockReportKind.Last => GetLastStockAsync(productCode, cancellationToken),
        StockReportKind.Monthly => GetMonthlyStockAsync(productCode, cancellationToken),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    private async Task<IReadOnlyList<StockReportItemDto>> GetCurrentStockAsync(decimal? productCode, CancellationToken cancellationToken = default)
    {
        var query = context.TCurrentStocks.AsNoTracking();
        if (productCode.HasValue)
        {
            query = query.WhereProductCodeStartsWith(stock => stock.CstProCd, productCode.Value);
        }

        return await query.OrderBy(stock => stock.CstProCd).Select(stock => new StockReportItemDto
        {
            ProductCode = stock.CstProCd,
            OpeningStock = stock.CstOpenStock,
            WarehouseNg = stock.CstWarehouseNg,
            Disposal = stock.CstDisposal,
            Repair = stock.CstRepair,
            Production = stock.CstProduction,
            Delivery = stock.CstDelivery,
            ClosingStock = stock.CstStock
        }).ToListAsync(cancellationToken);
    }

    private async Task<IReadOnlyList<StockReportItemDto>> GetLastStockAsync(decimal? productCode, CancellationToken cancellationToken = default)
    {
        var query = context.TLastStocks.AsNoTracking();
        if (productCode.HasValue)
        {
            query = query.WhereProductCodeStartsWith(stock => stock.LstProCd, productCode.Value);
        }

        return await query.OrderBy(stock => stock.LstProCd).Select(stock => new StockReportItemDto
        {
            ProductCode = stock.LstProCd,
            OpeningStock = stock.LstOpenStock,
            WarehouseNg = stock.LstWarehouseNg,
            Disposal = stock.LstDisposal,
            Repair = stock.LstRepair,
            Production = stock.LstProduction,
            Delivery = stock.LstDelivery,
            ClosingStock = stock.LstStock
        }).ToListAsync(cancellationToken);
    }

    private async Task<IReadOnlyList<StockReportItemDto>> GetMonthlyStockAsync(decimal? productCode, CancellationToken cancellationToken = default)
    {
        var query = context.TMonthlyStocks.AsNoTracking();
        if (productCode.HasValue)
        {
            query = query.WhereProductCodeStartsWith(stock => stock.MstProCd, productCode.Value);
        }

        return await query.OrderBy(stock => stock.MstDateReport).ThenBy(stock => stock.MstProCd)
            .ThenBy(stock => stock.MstId).Select(stock => new StockReportItemDto
            {
                Id = stock.MstId,
                ReportDate = stock.MstDateReport,
                ProductCode = stock.MstProCd,
                OpeningStock = stock.MstOpenStock,
                WarehouseNg = stock.MstWarehouseNg,
                Disposal = stock.MstDisposal,
                Repair = stock.MstRepair,
                Production = stock.MstProduction,
                Delivery = stock.MstDelivery,
                ClosingStock = stock.MstCloseStock
            }).ToListAsync(cancellationToken);
    }

    public async Task<StockReportHistoryDto> GetHistoryAsync(StockReportKind kind, decimal productCode, DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        var product = await (
            from p in context.TInputProducts.AsNoTracking()
            join c in context.TCustomers.AsNoTracking() on p.ProCusCd equals (decimal?)c.CusCd into customers
            from customer in customers.DefaultIfEmpty()
            where p.ProCd == productCode
            select new { p.ProName, CustomerName = customer == null ? null : customer.CusFullName }
        ).FirstOrDefaultAsync(cancellationToken);

        List<StockReportHistoryItemDto> items;
        if (kind == StockReportKind.Monthly)
        {
            // Report dates can be earlier than the last calendar day of the month.
            // Select the stored report month rather than the transaction/input date.
            items = await context.TMonthlyStockHistories.AsNoTracking()
                .Where(item => item.MsthProCd == productCode && item.MsthDateReport >= from && item.MsthDateReport <= to)
                .OrderBy(item => item.MsthWorkDate).ThenBy(item => item.MsthDateInput).ThenBy(item => item.MsthId)
                .Select(item => new StockReportHistoryItemDto
                {
                    Id = item.MsthId, ReceiveNo = item.MsthReceiveNo, ProductCode = item.MsthProCd,
                    Section = item.MsthSection, InputStock = item.MsthInputStock, OutputStock = item.MsthOutputStock,
                    CurrentStock = item.MsthCurrentStock, Remark = item.MsthRemark, WorkDate = item.MsthWorkDate,
                    DateInput = item.MsthDateInput, UserId = item.MsthUserId
                }).ToListAsync(cancellationToken);
        }
        else if (kind == StockReportKind.Last)
        {
            // LastStockHistory is the previous month's snapshot, just as CurrentStockHistory
            // is the current snapshot. Keep all of its entries for the exact product.
            items = await context.TLastStockHistories.AsNoTracking()
                .Where(item => item.LsthProCd == productCode)
                .OrderBy(item => item.LsthWorkDate).ThenBy(item => item.LsthDateInput).ThenBy(item => item.LsthId)
                .Select(item => new StockReportHistoryItemDto
                {
                    Id = item.LsthId, ReceiveNo = item.LsthReceiveNo, ProductCode = item.LsthProCd,
                    Section = item.LthSection, InputStock = item.LsthInputStock, OutputStock = item.LsthOutputStock,
                    CurrentStock = item.LsthCurrentStock, Remark = item.LsthRemark, WorkDate = item.LsthWorkDate,
                    DateInput = item.LsthDateInput, UserId = item.LsthUserId
                }).ToListAsync(cancellationToken);
        }

        else if (kind == StockReportKind.Current)
        {
            items = await context.TCurrentStockHistories.AsNoTracking()
                .Where(item => item.CsthProCd == productCode)
                .OrderBy(item => item.CsthWorkDate).ThenBy(item => item.CsthDateInput).ThenBy(item => item.CsthId)
                .Select(item => new StockReportHistoryItemDto
                {
                    Id = item.CsthId, ReceiveNo = item.CsthReceiveNo, ProductCode = item.CsthProCd,
                    Section = item.CsthSection, InputStock = item.CsthInputStock, OutputStock = item.CsthOutputStock,
                    CurrentStock = item.CsthCurrentStock, Remark = item.CsthRemark, WorkDate = item.CsthWorkDate,
                    DateInput = item.CsthDateInput, UserId = item.CsthUserId
                }).ToListAsync(cancellationToken);
        }
        else throw new ArgumentOutOfRangeException(nameof(kind));

        return new StockReportHistoryDto
        {
            ProductCode = productCode, ProductName = product?.ProName, CustomerName = product?.CustomerName,
            WorkingDayFrom = from, WorkingDayTo = to, Items = items
        };
    }
}
