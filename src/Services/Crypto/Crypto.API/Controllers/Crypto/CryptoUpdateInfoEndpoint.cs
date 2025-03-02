using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoUpdateInfoEndpoint : CryptoBaseController
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPatch(EndpointsConfigurations.CryptoEndpoints.UpdateInfo)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInfo([FromBody] UpdateInfoCommand instance)
    {
        await Mediator.Send(instance);
        return NoContent();
    }
}