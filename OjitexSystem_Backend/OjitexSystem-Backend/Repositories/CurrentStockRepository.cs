using Microsoft.EntityFrameworkCore;
using System.Globalization;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Interfaces;

namespace OjitexSystem_Backend.Repositories;

public class CurrentStockRepository : ICurrentStockRepository
{
    private readonly ProductionContext _context;

    public CurrentStockRepository(ProductionContext context)
    {
        _context = context;
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
