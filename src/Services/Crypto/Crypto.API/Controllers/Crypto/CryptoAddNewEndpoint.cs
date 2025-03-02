using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class CryptoAddNewEndpoint : CryptoBaseController
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost(EndpointsConfigurations.CryptoEndpoints.Create)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNew([FromBody] AddNewCommand instance)
    {
        await Mediator.Send(instance);
        return Created();
    }
}