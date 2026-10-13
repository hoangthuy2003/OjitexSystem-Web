using Microsoft.EntityFrameworkCore;
using System.Globalization;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Data.Auth;

namespace OjitexSystem_Backend.Repositories;

public class CurrentStockRepository : ICurrentStockRepository
{
    private readonly ProductionContext _context;
    private readonly AuthContext _authContext;

    public CurrentStockRepository(ProductionContext context, AuthContext authContext)
    {
        _context = context;
        _authContext = authContext;
    }

    public async Task<CurrentStockHistoryDto> GetHistoryByProductCodeAsync(decimal proCd)
    {
        var product = await (
            from p in _context.TInputProducts.AsNoTracking()
            join c in _context.TCustomers.AsNoTracking() on p.ProCusCd equals (decimal?)c.CusCd into customers
            from customer in customers.DefaultIfEmpty()
            where p.ProCd == proCd
            select new { p.ProName, CustomerName = customer == null ? null : customer.CusFullName }
        ).FirstOrDefaultAsync();

        var items = await _context.TCurrentStockHistories.AsNoTracking()
            .Where(history => history.CsthProCd == proCd)
            .OrderBy(history => history.CsthWorkDate)
            .ThenBy(history => history.CsthDateInput)
            .ThenBy(history => history.CsthId)
            .ToListAsync();

        var userIds = items
            .Where(item => !string.IsNullOrWhiteSpace(item.CsthUserId))
            .Select(item => item.CsthUserId!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var firstNames = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        if (userIds.Length > 0)
        {
            var users = await _authContext.IeUsers.AsNoTracking()
                .Where(user => userIds.Contains(user.UserId.Trim()))
                .Select(user => new { user.UserId, user.UserFirstName })
                .ToListAsync();
            foreach (var user in users)
                firstNames[user.UserId.Trim()] = user.UserFirstName?.Trim();
        }

        return new CurrentStockHistoryDto
        {
            ProductCode = proCd,
            ProductName = product?.ProName,
            CustomerName = product?.CustomerName,
            Items = items.Select(item => new CurrentStockHistoryItemDto
            {
                CsthId = item.CsthId,
                CsthReceiveNo = item.CsthReceiveNo,
                CsthProCd = item.CsthProCd,
                CsthSection = item.CsthSection,
                CsthInputStock = item.CsthInputStock,
                CsthOutputStock = item.CsthOutputStock,
                CsthCurrentStock = item.CsthCurrentStock,
                CsthRemark = item.CsthRemark,
                CsthWorkDate = item.CsthWorkDate,
                CsthDateInput = item.CsthDateInput,
                CsthUserId = item.CsthUserId,
                UserFirstName = firstNames.GetValueOrDefault(item.CsthUserId?.Trim() ?? "")
            }).ToList()
        };
    }

    public async Task<IEnumerable<TCurrentStock>> GetAllAsync()
    {
        return await _context.TCurrentStocks.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<TCurrentStock>> GetByProductCode(decimal proCd)
    {
        var prefix = proCd.ToString(CultureInfo.InvariantCulture);
            
        return await _context.TCurrentStocks.AsNoTracking()
            .Where(stock => stock.CstProCd.ToString().StartsWith(prefix))
            .OrderBy(stock => stock.CstProCd)
            .ToListAsync();              
    }       
}
