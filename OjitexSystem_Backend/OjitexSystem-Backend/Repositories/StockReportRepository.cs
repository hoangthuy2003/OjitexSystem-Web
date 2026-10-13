using System.Globalization;
using Microsoft.EntityFrameworkCore;
using OjitexSystem_Backend.Data.Auth;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Repositories;

public class StockReportRepository(ProductionContext context, AuthContext authContext) : IStockReportRepository
{
    public async Task<IReadOnlyList<StockReportItemDto>> GetLastStockAsync(decimal? productCode)
    {
        var query = context.TLastStocks.AsNoTracking();
        if (productCode.HasValue)
        {
            var prefix = productCode.Value.ToString(CultureInfo.InvariantCulture);
            query = query.Where(stock => stock.LstProCd.ToString().StartsWith(prefix));
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
        }).ToListAsync();
    }

    public async Task<IReadOnlyList<StockReportItemDto>> GetMonthlyStockAsync(decimal? productCode)
    {
        var query = context.TMonthlyStocks.AsNoTracking();
        if (productCode.HasValue)
        {
            var prefix = productCode.Value.ToString(CultureInfo.InvariantCulture);
            query = query.Where(stock => stock.MstProCd.HasValue && stock.MstProCd.Value.ToString().StartsWith(prefix));
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
            }).ToListAsync();
    }

    public async Task<StockReportHistoryDto> GetHistoryAsync(decimal productCode, DateOnly from, DateOnly to, bool monthly)
    {
        var product = await (
            from p in context.TInputProducts.AsNoTracking()
            join c in context.TCustomers.AsNoTracking() on p.ProCusCd equals (decimal?)c.CusCd into customers
            from customer in customers.DefaultIfEmpty()
            where p.ProCd == productCode
            select new { p.ProName, CustomerName = customer == null ? null : customer.CusFullName }
        ).FirstOrDefaultAsync();

        List<StockReportHistoryItemDto> items;
        if (monthly)
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
                }).ToListAsync();
        }
        else
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
                }).ToListAsync();
        }

        var userIds = items.Where(item => !string.IsNullOrWhiteSpace(item.UserId))
            .Select(item => item.UserId!.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        var firstNames = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        if (userIds.Length > 0)
        {
            var users = await authContext.IeUsers.AsNoTracking()
                .Where(user => userIds.Contains(user.UserId.Trim()))
                .Select(user => new { user.UserId, user.UserFirstName }).ToListAsync();
            foreach (var user in users)
                firstNames[user.UserId.Trim()] = user.UserFirstName?.Trim();
        }
        foreach (var item in items)
            item.UserFirstName = firstNames.GetValueOrDefault(item.UserId?.Trim() ?? "");

        return new StockReportHistoryDto
        {
            ProductCode = productCode, ProductName = product?.ProName, CustomerName = product?.CustomerName,
            WorkingDayFrom = from, WorkingDayTo = to, Items = items
        };
    }
}
