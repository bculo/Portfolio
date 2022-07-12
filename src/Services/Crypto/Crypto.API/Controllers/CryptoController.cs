using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Application.Modules.Crypto.Commands.Delete;
using Crypto.Application.Modules.Crypto.Commands.UpdatePrice;
using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Application.Modules.Crypto.Queries.FetchAll;
using Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ILogger<CryptoController> _logger;
        private readonly IMediator _mediator;

        public CryptoController(ILogger<CryptoController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> AddNewCrypto([FromBody] AddNewCommand instance)
        {
            await _mediator.Send(instance);
            return NoContent();
        }

        [HttpPost("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice([FromBody] UpdatePriceCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("UpdateAllPrices")]
        public async Task<IActionResult> UpdateAllPrices()
        {
            await _mediator.Send(new UpdatePriceAllCommand { });
            return NoContent();
        }

        [HttpGet("FetchAll")]
        public async Task<IActionResult> FetchAll()
        {
            return Ok(await _mediator.Send(new FetchAllQuery { }));
        }

        [HttpGet("Single/{symbol}")]
        public async Task<IActionResult> FetchSingle(string symbol)
        {
            return Ok(await _mediator.Send(new FetchSingleQuery { Symbol = symbol }));
        }

        [HttpDelete("Delete/{symbol}")]
        public async Task<IActionResult> Delete(string symbol)
        {
            await _mediator.Send(new DeleteCommand { Symbol = symbol });
            return NoContent();
        }

        [HttpGet("GetPriceHisotry/{symbol}")]
        public async Task<IActionResult> GetPriceHistory(string symbol)
        {
            return Ok(await _mediator.Send(new FetchPriceHistoryQuery { Symbol = symbol }));
        }
    }
}
