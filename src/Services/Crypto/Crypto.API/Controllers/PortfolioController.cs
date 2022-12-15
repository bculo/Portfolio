using Crypto.Application.Modules.Portfolio.Commands.Add;
using Crypto.Application.Modules.Portfolio.Queries.FetchAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly ILogger<PortfolioController> _logger;
        private readonly IMediator _mediator;

        public PortfolioController(ILogger<PortfolioController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("FetchAll")]
        public async Task<IActionResult> FetchAll()
        {
            return Ok(await _mediator.Send(new FetchAllPortfoliosQuery { }));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddPorftolioCommand instance)
        {
            return Ok(await _mediator.Send(instance));
        }
    }
}
