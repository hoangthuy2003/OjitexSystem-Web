using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Services.Interfaces;

public interface ICurrentStockService
{
    Task<IEnumerable<TCurrentStock>> GetAllAsync();

    Task<IEnumerable<TCurrentStock>> GetByProductCode(decimal proCd);
}
