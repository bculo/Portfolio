using Microsoft.AspNetCore.Mvc;

namespace Tracker.API.Endpoints.Portfolio;

[ApiExplorerSettings(GroupName = TrackerEndpointConfigurations.Portfolio.Label)]
public class FetchPortfolioItemsEndpoint : TrackerEndpoint
{
    [HttpGet(TrackerEndpointConfigurations.Portfolio.FetchItems)]
    public IActionResult FetchPortfolioItems(CancellationToken ct)
    {
        return Ok();
    }
}