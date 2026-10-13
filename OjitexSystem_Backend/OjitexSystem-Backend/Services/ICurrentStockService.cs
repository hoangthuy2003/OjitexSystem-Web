using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos;

namespace OjitexSystem_Backend.Services;

public interface ICurrentStockService
{
    Task<IEnumerable<TCurrentStock>> GetAllAsync();

    Task<IEnumerable<TCurrentStock>> GetByProductCode(decimal proCd);
    Task<CurrentStockHistoryDto> GetHistoryByProductCodeAsync(decimal proCd);
}
