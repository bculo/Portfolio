using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoUndoAddNewDelayEndpoint : CryptoBaseController
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost(EndpointsConfigurations.CryptoEndpoints.UndoDelayCreate)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UndoAddNewDelay([FromBody] UndoNewWithDelayCommand instance)
    {
        await Mediator.Send(instance);
        return NoContent();
    }
}