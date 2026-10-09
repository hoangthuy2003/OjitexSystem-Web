using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Repositories;

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

    public Task<IEnumerable<TCurrentStock>> GetByProductCode(decimal proCd)
    {
        return _repository.GetByProductCode(proCd);
    }
}
