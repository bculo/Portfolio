using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Stock.Application.Common.Constants;

namespace Stock.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CacheController : ControllerBase
{
    private readonly IOutputCacheStore _outputCache;

    public CacheController(IOutputCacheStore outputCache)
    {
        _outputCache = outputCache;
    }
    
    [HttpGet("Evict", Name = "Evict")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Evict(CancellationToken ct)
    {
        await _outputCache.EvictByTagAsync(CacheTags.STOCK_SINGLE, ct);

        return NoContent();
    }
}