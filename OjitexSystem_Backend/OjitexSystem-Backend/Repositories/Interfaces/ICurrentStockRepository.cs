using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Repositories.Interfaces;

public interface ICurrentStockRepository
{
    Task<IEnumerable<TCurrentStock>> GetAllAsync();

    Task<IEnumerable<TCurrentStock>> GetByProductCode(decimal proCd);
}
