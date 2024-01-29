using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Commands.Stock;
using Stock.Application.Queries.Stock;

namespace Stock.API.Controllers.v1
{
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
        
        [HttpPost("Create", Name = "CreateStock")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateStock createStock)
        {
            return Ok(await _mediator.Send(createStock));
        }

        [HttpGet("Single/{symbol}", Name = "GetStock")]
        [ProducesResponseType(typeof(GetStockResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStock([FromRoute] string symbol)
        {
            return Ok(await _mediator.Send(new GetStock(symbol)));
        }

        [HttpGet("All", Name = "GetStocks")]
        [ProducesResponseType(typeof(IEnumerable<GetStocksResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStocks()
        {
            return Ok(await _mediator.Send(new GetStocks()));
        }

        [HttpGet("Filter", Name = "FilterStocks")]
        [ProducesResponseType(typeof(IEnumerable<FilterStocks>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FilterStocks([FromQuery] FilterStocks filterListQuery)
        {
            return Ok(await _mediator.Send(filterListQuery));
        }
    }
}
