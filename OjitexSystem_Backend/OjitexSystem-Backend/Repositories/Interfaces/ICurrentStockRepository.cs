using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Repositories.Interfaces;

public interface ICurrentStockRepository
{
    Task<IEnumerable<TCurrentStock>> GetAllAsync();

    Task<TCurrentStock?> GetByProductCodeAsync(decimal proCd);

    Task<IEnumerable<TCurrentStock>> GetByProductCodeRangeAsync(decimal minimumCode, decimal maximumCode);
}
