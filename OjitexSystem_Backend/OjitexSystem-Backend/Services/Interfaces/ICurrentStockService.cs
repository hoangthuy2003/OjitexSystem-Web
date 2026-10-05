using OjitexSystem_Backend.Data.Production;

namespace OjitexSystem_Backend.Services.Interfaces;

public interface ICurrentStockService
{
    Task<IEnumerable<TCurrentStock>> GetAllAsync();

    Task<TCurrentStock?> GetByProductCodeAsync(decimal proCd);
}
