using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Controllers.Logistics;

[ApiController]
[Route("api/StockReport/{kind}")]
[Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
public class StockReportController(IStockReportService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<StockReportDto>> GetAll(StockReportKind kind, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(kind)) return BadRequest("Unknown stock report type.");
        return Ok(await service.GetStockAsync(kind, cancellationToken: cancellationToken));
    }

    [HttpGet("{productCode}")]
    public async Task<ActionResult<StockReportDto>> GetByProductCode(StockReportKind kind, decimal productCode, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(kind)) return BadRequest("Unknown stock report type.");
        if (!ValidProductCode(productCode)) return BadRequest("Product code must contain no more than 7 digits.");
        return Ok(await service.GetStockAsync(kind, productCode, cancellationToken));
    }

    [HttpGet("{productCode}/history")]
    public async Task<ActionResult<StockReportHistoryDto>> GetHistory(StockReportKind kind, decimal productCode, [FromQuery] DateOnly? reportDate, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(kind)) return BadRequest("Unknown stock report type.");
        if (!ValidProductCode(productCode)) return BadRequest("Product code must contain no more than 7 digits.");
        if (kind == StockReportKind.Monthly && !reportDate.HasValue)
            return BadRequest("Report date is required to select the stock history month.");
        return Ok(await service.GetHistoryAsync(kind, productCode, reportDate, cancellationToken));
    }

    private static bool ValidProductCode(decimal code) => code >= 0 && code <= 9999999 && decimal.Truncate(code) == code;
}
