using Crypto.Application.Common.Models;
using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoFetchPageEndpoint : CryptoBaseController
{
    [HttpGet(EndpointsConfigurations.CryptoEndpoints.Page)]
    [ProducesResponseType(typeof(PageBaseResult<FetchPageResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FetchPage([FromQuery] FetchPageQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}