using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Data.Production;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services;

namespace OjitexSystem_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
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
    public async Task<ActionResult<IEnumerable<TCurrentStock>>> GetByProductCode(decimal proCd)
    {
        var stocks = await _service.GetByProductCode(proCd);
        return Ok(stocks);
    }
}
