using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddNew")]
        public async Task<IActionResult> AddNew([FromBody] AddNew.Command command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("Single/{symbol}")]
        public async Task<IActionResult> GetSingle([FromRoute] string symbol)
        {
            return Ok(await _mediator.Send(new GetSingle.Query { Symbol = symbol }));
        }
    }
}
