using Crypto.Application.Modules.Crypto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoFetchSingleEndpoint : CryptoBaseController
{
    [HttpGet(EndpointsConfigurations.CryptoEndpoints.Single)]
    [ProducesResponseType(typeof(FetchSingleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FetchSingle(string symbol)
    {
        return Ok(await Mediator.Send(new FetchSingleQuery(symbol)));
    }
}