using Microsoft.AspNetCore.Mvc;

namespace Tracker.API.Endpoints.Portfolio;

[ApiExplorerSettings(GroupName = TrackerEndpointConfigurations.Portfolio.Label)]
public class FetchPortfolioItemEndpoint : TrackerEndpoint
{
    [HttpGet(TrackerEndpointConfigurations.Portfolio.FetchItem)]
    public IActionResult FetchPortfolioItem([FromRoute] Guid id, CancellationToken ct)
    {
        return Ok();
    }
}