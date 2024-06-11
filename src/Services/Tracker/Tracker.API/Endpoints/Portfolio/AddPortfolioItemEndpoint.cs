using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Features.Portfolio;

namespace Tracker.API.Endpoints.Portfolio;

[ApiExplorerSettings(GroupName = TrackerEndpointConfigurations.Portfolio.Label)]
public class AddPortfolioItemEndpoint : TrackerEndpoint
{
    [HttpPost(TrackerEndpointConfigurations.Portfolio.AddItem)]
    public async Task<IActionResult> AddPortfolioItem([FromBody] AddPortfolioItemCommand request, CancellationToken ct)
    {
        await Mediator.Send(request, ct);
        return NoContent();
    }
}