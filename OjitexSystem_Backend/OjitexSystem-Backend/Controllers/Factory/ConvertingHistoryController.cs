using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Factory;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Factory;

namespace OjitexSystem_Backend.Controllers.Factory;

[ApiController]
[Route("api/factory/converting-history")]
[Authorize(Policy = AuthorizationPolicies.ActiveUser)]
public sealed class ConvertingHistoryController(IConvertingOnlineService service) : ControllerBase
{
    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<ConvertingHistoryDto>> GetHistory(
        [FromQuery] string? date, [FromQuery] string? process, [FromQuery] string? productCode,
        CancellationToken cancellationToken)
    {
        DateOnly? reportDate = null;
        if (!string.IsNullOrWhiteSpace(date))
        {
            if (!DateOnly.TryParseExact(date.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid date in mm/dd/yyyy format." });
            }
            reportDate = parsed;
        }

        process = string.IsNullOrWhiteSpace(process) ? null : process.Trim();
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();
        if (process is { Length: > 6 })
        {
            return BadRequest(new { message = "Process must be at most 6 characters." });
        }
        if (!string.IsNullOrEmpty(productCode)
            && (productCode.Length > 7 || productCode.Any(character => character is < '0' or > '9')))
        {
            return BadRequest(new { message = "Product code must contain 1 to 7 digits." });
        }

        return Ok(await service.GetHistoryAsync(reportDate, process, productCode, cancellationToken));
    }
}
