using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Commands.Cache;
using Stock.Application.Common.Constants;

namespace Stock.API.Controllers.v1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CacheController(ISender mediator) : ControllerBase
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("EvictAll", Name = "EvictAll")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> EvictAll(CancellationToken ct)
    {
        await mediator.Send(new EvictAllOutputCache(), ct);
        return NoContent();
    }
}