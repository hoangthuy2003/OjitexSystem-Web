using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Dtos;
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

    public async Task<CurrentStockHistoryDto> GetHistoryByProductCodeAsync(decimal proCd)
    {
        var history = await _repository.GetHistoryByProductCodeAsync(proCd);
        // Report dates follow Vietnam time (UTC+7), regardless of the server's timezone.
        var today = DateOnly.FromDateTime(DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(7)).DateTime);
        history.WorkingDayFrom = new DateOnly(today.Year, today.Month, 1);
        history.WorkingDayTo = today;
        return history;
    }

    public Task<IEnumerable<TCurrentStock>> GetByProductCode(decimal proCd)
    {
        return _repository.GetByProductCode(proCd);
    }
}
