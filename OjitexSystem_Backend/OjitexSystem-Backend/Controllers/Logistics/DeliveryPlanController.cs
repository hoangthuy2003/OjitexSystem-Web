using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Controllers.Logistics;

[ApiController]
[Route("api/DeliveryPlan")]
[Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
public class DeliveryPlanController(IDeliveryPlanService service) : ControllerBase
{
    private const int MaxReceiveNoLength = 20;

    [HttpGet("search")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<DeliveryPlanSearchResultDto>> Search(
        [FromQuery] string? deliveryDate,
        [FromQuery] string? receiveNo,
        CancellationToken cancellationToken)
    {
        DateOnly? parsedDeliveryDate = null;
        if (!string.IsNullOrWhiteSpace(deliveryDate))
        {
            if (!DateOnly.TryParseExact(deliveryDate.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid date in MM/dd/yyyy format." });
            }
            parsedDeliveryDate = parsed;
        }

        receiveNo = string.IsNullOrWhiteSpace(receiveNo) ? null : receiveNo.Trim();

        if (receiveNo is { Length: > MaxReceiveNoLength })
        {
            return BadRequest(new { message = $"Receive No must contain at most {MaxReceiveNoLength} characters." });
        }

        return Ok(await service.SearchAsync(parsedDeliveryDate, receiveNo, cancellationToken));
    }
}
