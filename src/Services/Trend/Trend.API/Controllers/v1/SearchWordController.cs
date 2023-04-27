using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.SearchWord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Action;
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
        [ProducesResponseType(typeof(List<SearchWordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncSettingsWords()
        {
            return Ok(await _service.GetSyncSettingsWords());
        }

        [HttpGet("GetAvaiableSearchEngines")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvaiableSearchEngines()
        {
            return Ok(await _service.GetAvailableSearchEngines());
        }

        [HttpGet("GetAvaiableContextTypes")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvaiableContextTypes()
        {
            return Ok(await _service.GetAvaiableContextTypes());
        }

        [HttpPost("AddNewSearchWord")]
        [ProducesResponseType(typeof(SearchWordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewSearchWord([FromBody] SearchWordCreateDto request)
        {
            return Ok(await _service.AddNewSyncSetting(request));
        }

        [HttpDelete("RemoveSearchWord/{id}")]
        [ProducesResponseType(typeof(SearchWordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveSearchWord(string id)
        {
            await _service.RemoveSyncSetting(id);
            return NoContent();
        }
    }
}
