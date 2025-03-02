using Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoGetPriceHistoryEndpoint : CryptoBaseController
{
    [HttpGet(EndpointsConfigurations.CryptoEndpoints.History)]
    [ProducesResponseType(typeof(List<PriceHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FetchPriceHistory(Guid cryptoId)
    {
        return Ok(await Mediator.Send(new FetchPriceHistoryQuery { CryptoId = cryptoId }));
    }
}