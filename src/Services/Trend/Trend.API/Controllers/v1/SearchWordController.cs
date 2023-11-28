using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.SearchWord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;

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
        [ProducesResponseType(typeof(List<SearchWordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncSettingsWords(CancellationToken token)
        {
            return Ok(await _service.GetSyncSettingsWords(token));
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
        [ProducesResponseType(typeof(SearchWordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewSearchWord([FromBody] SearchWordCreateDto request, CancellationToken token)
        {
            return Ok(await _service.AddNewSyncSetting(request, token));
        }

        [HttpDelete("RemoveSearchWord/{id}")]
        [ProducesResponseType(typeof(SearchWordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveSearchWord(string id, CancellationToken token)
        {
            await _service.RemoveSyncSetting(id, token);
            return NoContent();
        }
    }
}
