using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchWordController : ControllerBase
    {
        private readonly ILogger<SearchWordController> _logger;
        private readonly ISearchWordService _service;

        public SearchWordController(ISearchWordService service, ILogger<SearchWordController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("GetSearchWords")]
        [ProducesResponseType(typeof(List<SearchWordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncSettingsWords()
        {
            _logger.LogTrace("Method GetSyncSettingsWords called in SyncController");

            return Ok(await _service.GetSyncSettingsWords());
        }

        [HttpGet("GetAvaiableSearchEngines")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvaiableSearchEngines()
        {
            _logger.LogTrace("Method GetAvaiableSearchEngines called in SyncController");

            return Ok(await _service.GetAvailableSearchEngines());
        }

        [HttpGet("GetAvaiableContextTypes")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvaiableContextTypes()
        {
            _logger.LogTrace("Method GetAvaiableContextTypes called in SyncController");

            return Ok(await _service.GetAvaiableContextTypes());
        }

        [HttpPost("AddNewSearchWord")]
        [ProducesResponseType(typeof(SearchWordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewSearchWord([FromBody] SearchWordCreateDto request)
        {
            _logger.LogTrace("Method AddNewSearchWord called in SyncController");

            return Ok(await _service.AddNewSyncSetting(request));
        }

        [HttpDelete("RemoveSearchWord/{id}")]
        [ProducesResponseType(typeof(SearchWordDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveSearchWord(string id)
        {
            _logger.LogTrace("Method RemoveSearchWord called in SyncController");

            await _service.RemoveSyncSetting(id);

            return NoContent();
        }
    }
}
