using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OjitexSystem_Backend.Dtos.Factory;
using OjitexSystem_Backend.Security;
using OjitexSystem_Backend.Services.Factory;

namespace OjitexSystem_Backend.Controllers.Factory;

[ApiController]
[Route("api/factory/fap-online")]
[Authorize(Policy = AuthorizationPolicies.ActiveUser)]
public sealed class FapOnlineController(IConvertingOnlineService service) : ControllerBase
{
    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<ActionResult<ConvertingOnlineDto>> GetToday(CancellationToken cancellationToken) =>
        Ok(await service.GetTodayAsync("FAP", cancellationToken));
}
