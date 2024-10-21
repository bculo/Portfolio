using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Models;
using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay;
using Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay;
using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Crypto.Application.Modules.Crypto.Commands.UpdatePrice;
using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.Application.Modules.Crypto.Queries.GetMostPopular;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers;

[Authorize]
[ApiController]
public class CryptoController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost(EndpointsConfigurations.CryptoEndpoints.Create)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNew([FromBody] AddNewCommand instance)
    {
        await mediator.Send(instance);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost(EndpointsConfigurations.CryptoEndpoints.CreateWithDelay)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNewWithDelay([FromBody] AddNewWithDelayCommand instance)
    {
        return Ok(await mediator.Send(instance));
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost(EndpointsConfigurations.CryptoEndpoints.UndoDelayCreate)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UndoAddNewDelay([FromBody] UndoNewWithDelayCommand instance)
    {
        await mediator.Send(instance);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut(EndpointsConfigurations.CryptoEndpoints.UpdateInfo)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInfo([FromBody] UpdateInfoCommand instance)
    {
        await mediator.Send(instance);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut(EndpointsConfigurations.CryptoEndpoints.UpdatePrice)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrice([FromBody] UpdatePriceCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut(EndpointsConfigurations.CryptoEndpoints.UpdatePriceAll)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAllPrices()
    {
        await mediator.Send(new UpdatePriceAllCommand { });
        return NoContent();
    }
    
    [HttpGet(EndpointsConfigurations.CryptoEndpoints.Page)]
    [ProducesResponseType(typeof(PageBaseResult<FetchPageResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FetchPage([FromQuery] FetchPageQuery query)
    {
        return Ok(await mediator.Send(query));
    }

    [HttpGet(EndpointsConfigurations.CryptoEndpoints.Single)]
    [ProducesResponseType(typeof(FetchSingleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FetchSingle(string symbol)
    {
        return Ok(await mediator.Send(new FetchSingleQuery { Symbol = symbol }));
    }
    
    [HttpGet(EndpointsConfigurations.CryptoEndpoints.History)]
    [ProducesResponseType(typeof(List<PriceHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPriceHistory(Guid cryptoId)
    {
        return Ok(await mediator.Send(new FetchPriceHistoryQuery { CryptoId = cryptoId }));
    }

    [HttpGet(EndpointsConfigurations.CryptoEndpoints.Popular)]
    [ProducesResponseType(typeof(List<GetMostPopularResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMostPopular([FromQuery] GetMostPopularQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}

