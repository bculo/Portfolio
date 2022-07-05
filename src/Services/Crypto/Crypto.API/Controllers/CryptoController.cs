using Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto;
using Crypto.Application.Modules.Crypto.Queries.FetchAllCryptos;
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
    }
}
