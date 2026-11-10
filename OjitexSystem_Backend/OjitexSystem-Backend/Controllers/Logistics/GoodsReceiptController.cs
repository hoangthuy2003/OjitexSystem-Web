using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Controllers.Logistics;

[ApiController]
[Route("api/GoodsReceipt")]
[Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
public class GoodsReceiptController(IGoodsReceiptService service) : ControllerBase
{
    private const int MaxReceiveNoLength = 20;
    private const int MaxProductCodeLength = 7;

    [HttpGet("search")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<GoodsReceiptSearchResultDto>> Search(
        [FromQuery] string? dateReceipt,
        [FromQuery] string? receiveNo,
        CancellationToken cancellationToken)
    {
        DateOnly? parsedDateReceipt = null;
        if (!string.IsNullOrWhiteSpace(dateReceipt))
        {
            if (!DateOnly.TryParseExact(dateReceipt.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid date in MM/dd/yyyy format." });
            }
            parsedDateReceipt = parsed;
        }

        receiveNo = string.IsNullOrWhiteSpace(receiveNo) ? null : receiveNo.Trim();

        if (receiveNo is { Length: > MaxReceiveNoLength })
        {
            return BadRequest(new { message = $"Receive No must contain at most {MaxReceiveNoLength} characters." });
        }

        return Ok(await service.SearchAsync(parsedDateReceipt, receiveNo, cancellationToken));
    }

    [HttpGet("information")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<GoodsReceiptInformationDto>> GetInformation(
        [FromQuery] string? dateReceipt,
        [FromQuery] string? receiveNo,
        [FromQuery] string? productCode,
        CancellationToken cancellationToken)
    {
        DateOnly? parsedDateReceipt = null;
        if (!string.IsNullOrWhiteSpace(dateReceipt))
        {
            if (!DateOnly.TryParseExact(dateReceipt.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid date in MM/dd/yyyy format." });
            }
            parsedDateReceipt = parsed;
        }

        receiveNo = string.IsNullOrWhiteSpace(receiveNo) ? null : receiveNo.Trim();
        productCode = string.IsNullOrWhiteSpace(productCode) ? null : productCode.Trim();

        if (receiveNo is { Length: > MaxReceiveNoLength })
        {
            return BadRequest(new { message = $"Receive No must contain at most {MaxReceiveNoLength} characters." });
        }

        if (!string.IsNullOrEmpty(productCode)
            && (productCode.Length > MaxProductCodeLength || productCode.Any(c => c is < '0' or > '9')))
        {
            return BadRequest(new { message = $"Product code must contain 1 to {MaxProductCodeLength} digits." });
        }

        return Ok(await service.GetInformationAsync(parsedDateReceipt, receiveNo, productCode, cancellationToken));
    }
}
