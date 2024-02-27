using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay;
using Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay;
using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Crypto.Application.Modules.Crypto.Commands.UpdatePrice;
using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.Application.Modules.Crypto.Queries.GetMostPopular;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CryptoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddNew")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNew([FromBody] AddNewCommand instance)
        {
            await _mediator.Send(instance);
            return NoContent();
        }

        [HttpPost("AddNewWithDelay")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewWithDelay([FromBody] AddNewWithDelayCommand instance)
        {
            return Ok(await _mediator.Send(instance));
        }

        [HttpPost("UndoAddNewDelay")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UndoAddNewDelay([FromBody] UndoNewWithDelayCommand instance)
        {
            await _mediator.Send(instance);
            return NoContent();
        }

        [HttpPut("UpdateInfo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateInfoCommand instance)
        {
            await _mediator.Send(instance);
            return NoContent();
        }

        [HttpPut("UpdatePrice")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePrice([FromBody] UpdatePriceCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("UpdateAllPrices")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAllPrices()
        {
            await _mediator.Send(new UpdatePriceAllCommand { });
            return NoContent();
        }
        
        [HttpGet("FetchPage")]
        public async Task<IActionResult> FetchPage([FromQuery] FetchPageQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("Single/{symbol}")]
        [ProducesResponseType(typeof(FetchSingleResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FetchSingle(string symbol)
        {
            return Ok(await _mediator.Send(new FetchSingleQuery { Symbol = symbol }));
        }
        
        [HttpGet("GetPriceHistory/{cryptoId}")]
        [ProducesResponseType(typeof(List<PriceHistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPriceHistory(Guid cryptoId)
        {
            return Ok(await _mediator.Send(new FetchPriceHistoryQuery { CryptoId = cryptoId }));
        }

        [HttpGet("GetMostPopular")]
        [ProducesResponseType(typeof(List<GetMostPopularResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMostPopular([FromQuery] GetMostPopularQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
