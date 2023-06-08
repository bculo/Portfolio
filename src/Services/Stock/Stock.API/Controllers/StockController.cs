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
        public async Task<IActionResult> Get([FromBody] AddNew.Command command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
