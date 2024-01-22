using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features;

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

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] AddNewStockCommand addNewStockCommand)
        {
            return Ok(await _mediator.Send(addNewStockCommand));
        }

        [HttpGet("Single/{symbol}")]
        public async Task<IActionResult> GetSingle([FromRoute] string symbol)
        {
            return Ok(await _mediator.Send(new GetSingleQuery(symbol)));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllQuery()));
        }

        [HttpPost("FilterList")]
        public async Task<IActionResult> FilterList([FromBody] FilterListQuery filterListQuery)
        {
            return Ok(await _mediator.Send(filterListQuery));
        }
    }
}
