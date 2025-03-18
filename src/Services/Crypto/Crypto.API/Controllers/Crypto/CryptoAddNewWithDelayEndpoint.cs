using Crypto.Application.Common.Constants;
using Crypto.Application.Modules.Crypto.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Crypto;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.CryptoEndpoints.GroupTag)]
public class AddNewWithDelayEndpoint : CryptoBaseController
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost(EndpointsConfigurations.CryptoEndpoints.CreateWithDelay)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNewWithDelay([FromBody] AddNewWithDelayCommand instance)
    {
        return Accepted(await Mediator.Send(instance));
    }
}