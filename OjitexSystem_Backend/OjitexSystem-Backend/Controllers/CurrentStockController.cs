using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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

    // GET: api/CurrentStock/search/100000
    [HttpGet("search/{proCdPrefix}")]
    public async Task<ActionResult<IEnumerable<TCurrentStock>>> SearchByProductCodePrefix(
        string proCdPrefix)
    {
        if (proCdPrefix.Length is < 1 or > 7 ||
            proCdPrefix.Any(character => character is < '0' or > '9') ||
            !decimal.TryParse(proCdPrefix, NumberStyles.None, CultureInfo.InvariantCulture, out var prefix))
        {
            return BadRequest("Product code prefix must contain 1 to 7 digits.");
        }

        var remainingDigits = 7 - proCdPrefix.Length;
        var scale = (decimal)Math.Pow(10, remainingDigits);
        var minimumCode = prefix * scale;
        var maximumCode = minimumCode + scale - 1;
        var stocks = await _service.GetByProductCodeRangeAsync(minimumCode, maximumCode);

        return Ok(stocks);
    }
}
