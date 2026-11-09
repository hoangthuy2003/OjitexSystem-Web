using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Controllers.Logistics;

[ApiController]
[Route("api/DeliveryNote")]
[Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
public class DeliveryNoteController(IDeliveryNoteService service) : ControllerBase
{
    private const int MaxSlipLength = 30;
    private const int MaxCustomerCodeLength = 3;
    private const int MaxProductCodeLength = 7;

    [HttpGet("information")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<DeliveryInformationDto>> GetInformation(
        [FromQuery] string? deliverySlip,
        [FromQuery] string? customerCode,
        [FromQuery] string? productCode,
        [FromQuery] string? deliveryDate,
        CancellationToken cancellationToken)
    {
        DateOnly? parsedDeliveryDate = null;
        if (!string.IsNullOrWhiteSpace(deliveryDate))
        {
            if (!DateOnly.TryParseExact(deliveryDate.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid delivery date in mm/dd/yyyy format." });
            }
            parsedDeliveryDate = parsed;
        }

        deliverySlip = string.IsNullOrWhiteSpace(deliverySlip) ? null : deliverySlip.Trim();
        customerCode = string.IsNullOrWhiteSpace(customerCode) ? null : customerCode.Trim();
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();

        if (deliverySlip is { Length: > MaxSlipLength })
        {
            return BadRequest(new { message = $"Delivery slip must contain at most {MaxSlipLength} characters." });
        }

        if (!string.IsNullOrEmpty(customerCode)
            && (customerCode.Length > MaxCustomerCodeLength || customerCode.Any(c => c is < '0' or > '9')))
        {
            return BadRequest(new { message = $"Customer code must contain 1 to {MaxCustomerCodeLength} digits." });
        }

        if (!string.IsNullOrEmpty(productCode)
            && (productCode.Length > MaxProductCodeLength || productCode.Any(c => c is < '0' or > '9')))
        {
            return BadRequest(new { message = $"Product code must contain 1 to {MaxProductCodeLength} digits." });
        }

        return Ok(await service.GetInformationAsync(
            deliverySlip, customerCode, productCode, parsedDeliveryDate, cancellationToken));
    }

    [HttpGet("monthly")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<DeliveryMonthlyReportDto>> GetMonthly(
        [FromQuery] string? fromDate,
        [FromQuery] string? toDate,
        [FromQuery] string? deliverySlip,
        [FromQuery] string? customerCode,
        [FromQuery] string? productCode,
        CancellationToken cancellationToken)
    {
        DateOnly? parsedFromDate = null;
        if (!string.IsNullOrWhiteSpace(fromDate))
        {
            if (!DateOnly.TryParseExact(fromDate.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid from date in MM/dd/yyyy format." });
            }
            parsedFromDate = parsed;
        }

        DateOnly? parsedToDate = null;
        if (!string.IsNullOrWhiteSpace(toDate))
        {
            if (!DateOnly.TryParseExact(toDate.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid to date in MM/dd/yyyy format." });
            }
            parsedToDate = parsed;
        }

        if (parsedFromDate.HasValue && parsedToDate.HasValue && parsedFromDate.Value > parsedToDate.Value)
        {
            return BadRequest(new { message = "From date must be earlier than or equal to to date." });
        }

        deliverySlip = string.IsNullOrWhiteSpace(deliverySlip) ? null : deliverySlip.Trim();
        customerCode = string.IsNullOrWhiteSpace(customerCode) ? null : customerCode.Trim();
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();

        if (deliverySlip is { Length: > MaxSlipLength })
        {
            return BadRequest(new { message = $"Delivery slip must contain at most {MaxSlipLength} characters." });
        }

        if (!string.IsNullOrEmpty(customerCode)
            && (customerCode.Length > MaxCustomerCodeLength || customerCode.Any(c => c is < '0' or > '9')))
        {
            return BadRequest(new { message = $"Customer code must contain 1 to {MaxCustomerCodeLength} digits." });
        }

        if (!string.IsNullOrEmpty(productCode)
            && (productCode.Length > MaxProductCodeLength || productCode.Any(c => c is < '0' or > '9')))
        {
            return BadRequest(new { message = $"Product code must contain 1 to {MaxProductCodeLength} digits." });
        }

        return Ok(await service.GetMonthlyAsync(
            deliverySlip, customerCode, productCode, parsedFromDate, parsedToDate, cancellationToken));
    }

    [HttpGet("{deliverySlip}")]
    public async Task<ActionResult<DeliveryNoteDto>> Search(string deliverySlip, CancellationToken cancellationToken)
    {
        var slip = deliverySlip.Trim();
        if (slip.Length == 0 || slip.Length > MaxSlipLength)
            return BadRequest($"Delivery slip must contain 1 to {MaxSlipLength} characters.");
        var note = await service.SearchAsync(slip, cancellationToken);
        return note is null ? NotFound("Delivery note not found.") : Ok(note);
    }
}
