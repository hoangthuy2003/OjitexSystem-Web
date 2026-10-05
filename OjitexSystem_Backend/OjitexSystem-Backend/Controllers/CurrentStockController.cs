using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Services.Interfaces;

namespace OjitexSystem_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrentStockController : ControllerBase
{
    private readonly ICurrentStockService _service;

    public CurrentStockController(ICurrentStockService service)
    {
        _service = service;
    }

    // GET: api/CurrentStock
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TCurrentStock>>> GetAll()
    {
        var stocks = await _service.GetAllAsync();
        return Ok(stocks);
    }

    // GET: api/CurrentStock/1234567
    [HttpGet("{proCd}")]
    public async Task<ActionResult<TCurrentStock>> GetByProductCode(decimal proCd)
    {
        var stock = await _service.GetByProductCodeAsync(proCd);

        if (stock is null)
        {
            return NotFound();
        }

        return Ok(stock);
    }
}
