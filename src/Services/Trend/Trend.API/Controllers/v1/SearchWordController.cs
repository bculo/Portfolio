using Dtos.Common;
using Dtos.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;

namespace Trend.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SearchWordController : ControllerBase
    {
        private readonly ISearchWordService _service;

        public SearchWordController(ISearchWordService service)
        {
            _service = service;
        }

        [HttpGet("GetSearchWords")]
        [OutputCache(PolicyName = "SearchWordPolicy")]
        [ProducesResponseType(typeof(List<SearchWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncSettingsWords(CancellationToken token)
        {
            return Ok(await _service.GetSearchWords(token));
        }

        [HttpGet("GetAvailableSearchEngines")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableSearchEngines(CancellationToken token)
        {
            return Ok(await _service.GetAvailableSearchEngines(token));
        }

        [HttpGet("GetAvailableContextTypes")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableContextTypes(CancellationToken token)
        {
            return Ok(await _service.GetAvailableContextTypes(token));
        }

        [HttpPost("AddNewSearchWord")]
        [ProducesResponseType(typeof(SearchWordResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewSearchWord([FromBody] SearchWordAddReqDto request, CancellationToken token)
        {
            return Ok(await _service.AddNewSearchWord(request, token));
        }
        
        [HttpPost("AttachImageToSearchWord/{searchWordId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AttachImageToSearchWord([FromForm] FileUploadDto request, 
            [FromRoute] string searchWordId,
            CancellationToken token)
        {
            var command = await request.File.ToDetailsDto<SearchWordAttachImageReqDto>();
            command.SearchWordId = searchWordId;
            await _service.AttachImageToSearchWord(command, token);
            return NoContent();
        }

        [HttpDelete("Deactivate/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deactivate(string id, CancellationToken token)
        {
            await _service.DeactivateSearchWord(id, token);
            return NoContent();
        }
        
        [HttpGet("Activate/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Activate(string id, CancellationToken token)
        {
            await _service.ActivateSearchWord(id, token);
            return NoContent();
        }
    }
}
