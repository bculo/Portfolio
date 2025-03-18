using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoUpdatePriceEndpoint : CryptoBaseController
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch(EndpointsConfigurations.CryptoEndpoints.UpdatePrice)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrice([FromBody] UpdatePriceCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}