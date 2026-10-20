using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Factory;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Factory;

namespace OjitexSystem_Backend.Controllers.Factory;

[ApiController]
[Route("api/factory/corrugator-history")]
[Authorize(Policy = AuthorizationPolicies.ActiveUser)]
public sealed class CorrugatorHistoryController(ICorrugatorOnlineService service) : ControllerBase
{
    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<CorrugatorOnlineDto>> GetHistory(
        [FromQuery] string? date, CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var reportDate))
        {
            return BadRequest(new { message = "Enter a valid Corrugator date in mm/dd/yyyy format." });
        }

        return Ok(await service.GetHistoryAsync(reportDate, cancellationToken));
    }
}
