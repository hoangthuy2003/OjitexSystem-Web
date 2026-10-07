using Microsoft.EntityFrameworkCore;
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

    public async Task<TCurrentStock?> GetByProductCodeAsync(decimal proCd)
    {
        return await _context.TCurrentStocks.AsNoTracking()
            .FirstOrDefaultAsync(s => s.CstProCd == proCd);
    }

    public async Task<IEnumerable<TCurrentStock>> GetByProductCodeRangeAsync(
        decimal minimumCode,
        decimal maximumCode)
    {
        return await _context.TCurrentStocks.AsNoTracking()
            .Where(s => s.CstProCd >= minimumCode && s.CstProCd <= maximumCode)
            .OrderBy(s => s.CstProCd)
            .ToListAsync();
    }
}
