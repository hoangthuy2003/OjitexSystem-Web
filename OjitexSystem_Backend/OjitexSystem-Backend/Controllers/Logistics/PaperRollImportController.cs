using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Logistics;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Logistics;

namespace OjitexSystem_Backend.Controllers.Logistics;

[ApiController]
[Route("api/PaperRollImport")]
[Authorize(Policy = AuthorizationPolicies.LogisticsCategory)]
public sealed class PaperRollImportController(IPaperRollImportService service) : ControllerBase
{
    private const int MaxPaperLabelLength = 10;
    private const int MaxGradeNameLength = 15;

    [HttpGet("search")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<PaperRollImportSearchResultDto>> Search(
        [FromQuery] string? paperLabel,
        [FromQuery] string? gradeName,
        [FromQuery] string? dateReceived,
        CancellationToken cancellationToken)
    {
        DateOnly? parsedDateReceived = null;
        if (!string.IsNullOrWhiteSpace(dateReceived))
        {
            if (!DateOnly.TryParseExact(dateReceived.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsed))
            {
                return BadRequest(new { message = "Enter a valid date in MM/dd/yyyy format." });
            }
            parsedDateReceived = parsed;
        }

        paperLabel = string.IsNullOrWhiteSpace(paperLabel) ? null : paperLabel.Trim();
        gradeName = string.IsNullOrWhiteSpace(gradeName) ? null : gradeName.Trim();

        if (paperLabel is { Length: > MaxPaperLabelLength })
        {
            return BadRequest(new { message = $"Paper Label must contain at most {MaxPaperLabelLength} characters." });
        }

        if (gradeName is { Length: > MaxGradeNameLength })
        {
            return BadRequest(new { message = $"Grade Name must contain at most {MaxGradeNameLength} characters." });
        }

        return Ok(await service.SearchAsync(paperLabel, gradeName, parsedDateReceived, cancellationToken));
    }
}
