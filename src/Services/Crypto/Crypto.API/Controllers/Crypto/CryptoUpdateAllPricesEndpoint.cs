using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoUpdateAllPricesEndpoint : CryptoBaseController
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch(EndpointsConfigurations.CryptoEndpoints.UpdatePriceAll)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAllPrices()
    {
        await Mediator.Send(new UpdatePriceAllCommand());
        return NoContent();
    }
}