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
