using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services;

namespace OjitexSystem_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
public class LastStockController(IStockReportService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<StockReportDto>> GetAll() => Ok(await service.GetLastStockAsync());

    [HttpGet("{productCode}")]
    public async Task<ActionResult<StockReportDto>> GetByProductCode(decimal productCode)
    {
        if (!ValidProductCode(productCode)) return BadRequest("Product code must contain no more than 7 digits.");
        return Ok(await service.GetLastStockAsync(productCode));
    }

    [HttpGet("{productCode}/history")]
    public async Task<ActionResult<StockReportHistoryDto>> GetHistory(decimal productCode)
    {
        if (!ValidProductCode(productCode)) return BadRequest("Product code must contain no more than 7 digits.");
        return Ok(await service.GetLastHistoryAsync(productCode));
    }

    internal static bool ValidProductCode(decimal code) => code >= 0 && code <= 9999999 && decimal.Truncate(code) == code;
}
