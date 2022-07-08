using Crypto.Application.Interfaces.Services;
using Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto;
using Crypto.Application.Modules.Crypto.Commands.DeleteCrypto;
using Crypto.Application.Modules.Crypto.Queries.FetchAllCryptos;
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
        public async Task<IActionResult> AddNewCrypto([FromBody] AddNewCryptoCommand instance)
        {
            await _mediator.Send(instance);
            return NoContent();
        }

        [HttpGet("FetchAll")]
        public async Task<IActionResult> FetchAll()
        {
            return Ok(await _mediator.Send(new FetchAllCryptosQuery { }));
        }

        [HttpGet("Single/{symbol}")]
        public async Task<IActionResult> FetchSingle(string symbol)
        {
            return Ok(await _mediator.Send(new FetchSingleQuery { Symbol = symbol }));
        }

        [HttpDelete("Delete/{symbol}")]
        public async Task<IActionResult> Delete(string symbol)
        {
            await _mediator.Send(new DeleteCryptoCommand { Symbol = symbol });
            return NoContent();
        }
    }
}
