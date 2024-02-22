﻿using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Application.Modules.Crypto.Commands.AddNewWithDelay;
using Crypto.Application.Modules.Crypto.Commands.Delete;
using Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay;
using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Crypto.Application.Modules.Crypto.Commands.UpdatePrice;
using Crypto.Application.Modules.Crypto.Commands.UpdatePriceAll;
using Crypto.Application.Modules.Crypto.Queries.FetchGroup;
using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.Application.Modules.Crypto.Queries.GetMostPopular;
using Crypto.Application.Modules.Crypto.Queries.SearchBySymbol;
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
        
        [HttpPost("FetchPage")]
        public async Task<IActionResult> FetchPage([FromBody] FetchPageQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("Single/{symbol}")]
        public async Task<IActionResult> FetchSingle(string symbol)
        {
            return Ok(await _mediator.Send(new FetchSingleQuery { Symbol = symbol }));
        }

        [HttpPost("FetchGroup")]
        public async Task<IActionResult> FetchGroup([FromBody] FetchGroupQuery query)
        {
            return Ok(await _mediator.Send(query));
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

        [HttpPost("GetMostPopular")]
        public async Task<IActionResult> GetMostPopular([FromBody] GetMostPopularQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("SearchBySymbol")]
        public async Task<IActionResult> SearchBySymbol([FromBody] SearchBySymbolQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
