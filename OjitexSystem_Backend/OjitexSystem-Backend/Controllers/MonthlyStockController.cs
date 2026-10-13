using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services;

namespace OjitexSystem_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
public class MonthlyStockController(IStockReportService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<StockReportDto>> GetAll() => Ok(await service.GetMonthlyStockAsync());

    [HttpGet("{productCode}")]
    public async Task<ActionResult<StockReportDto>> GetByProductCode(decimal productCode)
    {
        if (!LastStockController.ValidProductCode(productCode)) return BadRequest("Product code must contain no more than 7 digits.");
        return Ok(await service.GetMonthlyStockAsync(productCode));
    }

    [HttpGet("{productCode}/history")]
    public async Task<ActionResult<StockReportHistoryDto>> GetHistory(decimal productCode, [FromQuery] DateOnly? reportDate)
    {
        if (!LastStockController.ValidProductCode(productCode)) return BadRequest("Product code must contain no more than 7 digits.");
        if (!reportDate.HasValue) return BadRequest("Report date is required to select the stock history month.");
        return Ok(await service.GetMonthlyHistoryAsync(productCode, reportDate.Value));
    }
}
