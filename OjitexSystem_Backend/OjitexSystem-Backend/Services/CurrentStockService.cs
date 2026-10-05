using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories.Interfaces;
using OjitexSystem_Backend.Services.Interfaces;

namespace OjitexSystem_Backend.Services;

public class CurrentStockService : ICurrentStockService
{
    private readonly ICurrentStockRepository _repository;

    public CurrentStockService(ICurrentStockRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<TCurrentStock>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<TCurrentStock?> GetByProductCodeAsync(decimal proCd)
    {
        return _repository.GetByProductCodeAsync(proCd);
    }
}
