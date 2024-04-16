using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Stock.API.Common.Constants;
using Stock.Application.Commands.Stock;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Models;
using Stock.Application.Queries.Stock;

namespace Stock.API.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Roles = AppRoles.ADMIN)]
    [HttpPost("Create", Name = "CreateStock")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStock createStock)
    {
        return Ok(await _mediator.Send(createStock));
    }
    
    [HttpGet("Single/{id}", Name = "GetStock")]
    [OutputCache(PolicyName = CachePolicies.STOCK_GET_SINGLE)]
    [ProducesResponseType(typeof(GetStockByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStock([FromRoute] string id)
    {
        return Ok(await _mediator.Send(new GetStockById(id)));
    }

    [HttpGet("All", Name = "GetStocks")]
    [OutputCache(PolicyName = CachePolicies.STOCK_GET_FILTER)]
    [ProducesResponseType(typeof(IEnumerable<GetStocksResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStocks()
    {
        return Ok(await _mediator.Send(new GetStocks()));
    }

    [HttpGet("Filter", Name = "FilterStocks")]
    [OutputCache(PolicyName = CachePolicies.STOCK_GET_FILTER)]
    [ProducesResponseType(typeof(PageResultDto<FilterStockResponseItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FilterStocks([FromQuery] FilterStocks filterListQuery)
    {
        return Ok(await _mediator.Send(filterListQuery));
    }
    
    [Authorize(Roles = AppRoles.ADMIN)]
    [HttpPut("ChangeStatus", Name = "ChangeStockStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeActivityStatus([FromBody] ChangeStockStatus request)
    {
        await _mediator.Send(request);
        return NoContent();
    }
}

