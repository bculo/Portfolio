using Crypto.Application.Modules.Crypto.Queries.GetMostPopular;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class FetchMostPopular : CryptoBaseController
{
    [HttpGet(EndpointsConfigurations.CryptoEndpoints.Popular)]
    [ProducesResponseType(typeof(List<GetMostPopularResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMostPopular([FromQuery] GetMostPopularQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}