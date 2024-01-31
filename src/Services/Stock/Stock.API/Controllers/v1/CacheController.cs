using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Commands.Cache;

namespace Stock.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CacheController : ControllerBase
{
    private readonly IMediator _mediator;

    public CacheController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpDelete("EvictAll", Name = "EvictAll")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> EvictAll(CancellationToken ct)
    {
        await _mediator.Send(new EvictAllOutputCache(), ct);
        return NoContent();
    }
}