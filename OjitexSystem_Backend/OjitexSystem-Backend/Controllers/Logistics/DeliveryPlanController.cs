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
            if (!TryParseDate(deliveryDate.Trim(), out var parsed))
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

    [HttpGet("monthly")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<DeliveryPlanMonthlyResultDto>> GetMonthly(
        [FromQuery] string? fromDate,
        [FromQuery] string? toDate,
        [FromQuery] string? productCode,
        CancellationToken cancellationToken)
    {
        DateOnly? parsedFromDate = null;
        if (!string.IsNullOrWhiteSpace(fromDate))
        {
            if (!TryParseDate(fromDate.Trim(), out var parsed))
            {
                return BadRequest(new { message = "Enter a valid from date in MM/dd/yyyy format." });
            }
            parsedFromDate = parsed;
        }

        DateOnly? parsedToDate = null;
        if (!string.IsNullOrWhiteSpace(toDate))
        {
            if (!TryParseDate(toDate.Trim(), out var parsed))
            {
                return BadRequest(new { message = "Enter a valid to date in MM/dd/yyyy format." });
            }
            parsedToDate = parsed;
        }

        if (parsedFromDate.HasValue && parsedToDate.HasValue && parsedFromDate.Value > parsedToDate.Value)
        {
            return BadRequest(new { message = "From date must be earlier than or equal to to date." });
        }

        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();
        if (productCode is { Length: > 7 } || (productCode != null && productCode.Any(c => c is < '0' or > '9')))
        {
            return BadRequest(new { message = "Product code must contain 1 to 7 digits." });
        }

        return Ok(await service.GetMonthlyAsync(parsedFromDate, parsedToDate, productCode, cancellationToken));
    }

    private static bool TryParseDate(string input, out DateOnly date) =>
        DateOnly.TryParseExact(input, ["MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd"],
            CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
}
