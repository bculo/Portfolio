using Microsoft.AspNetCore.Mvc;

namespace Tracker.API.Endpoints.Portfolio;

[ApiExplorerSettings(GroupName = TrackerEndpointConfigurations.Portfolio.Label)]
public class AddPortfolioItemEndpoint : TrackerEndpoint
{
    [HttpPost(TrackerEndpointConfigurations.Portfolio.AddItem)]
    public IActionResult AddPortfolioItem(CancellationToken ct)
    {
        return Ok();
    }
}