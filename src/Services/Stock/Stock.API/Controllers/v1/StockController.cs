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
        
        [HttpPost(nameof(AddNew), Name = nameof(AddNew))]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddNew([FromBody] AddNewStockCommand addNewStockCommand)
        {
            return Ok(await _mediator.Send(addNewStockCommand));
        }

        [HttpGet("Single/{symbol}", Name = nameof(GetSingle))]
        [ProducesResponseType(typeof(GetSingleResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle([FromRoute] string symbol)
        {
            return Ok(await _mediator.Send(new GetSingleQuery(symbol)));
        }

        [HttpGet("GetAll", Name = nameof(GetAll))]
        [ProducesResponseType(typeof(IEnumerable<GetAllResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllQuery()));
        }

        [HttpPost("FilterList", Name = nameof(FilterList))]
        [ProducesResponseType(typeof(IEnumerable<FilterListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FilterList([FromBody] FilterListQuery filterListQuery)
        {
            return Ok(await _mediator.Send(filterListQuery));
        }
    }
}
