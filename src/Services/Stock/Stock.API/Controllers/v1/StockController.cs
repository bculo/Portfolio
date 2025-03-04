﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Commands.Stock;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Models;
using Stock.Application.Queries.Stock;

namespace Stock.API.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StockController(ISender mediator) : ControllerBase
{
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost("Create", Name = "CreateStock")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStock createStock)
    {
        return Ok(await mediator.Send(createStock));
    }
    
    [HttpGet("Single/{id}", Name = "GetStock")]
    [ProducesResponseType(typeof(GetStockByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStock([FromRoute] Guid id)
    {
        return Ok(await mediator.Send(new GetStockByIdQuery(id)));
    }

    [HttpGet("All", Name = "GetStocks")]
    [ProducesResponseType(typeof(IEnumerable<GetStocksResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStocks()
    {
        return Ok(await mediator.Send(new GetStocksQuery()));
    }

    [HttpGet("Filter", Name = "FilterStocks")]
    [ProducesResponseType(typeof(PageResultDto<FilterStockResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FilterStocks([FromQuery] FilterStocksQuery filterListQuery)
    {
        return Ok(await mediator.Send(filterListQuery));
    }
    
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("ChangeStatus", Name = "ChangeStockStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeActivityStatus([FromBody] ChangeStockStatus request)
    {
        await mediator.Send(request);
        return NoContent();
    }
}

