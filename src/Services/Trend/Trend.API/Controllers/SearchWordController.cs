using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.SearchWord;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Models;
using Trend.API.Filters.Action;
using Trend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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


        [ServiceFilter(typeof(CacheActionFilter))]
        [HttpGet("GetAvaiableSearchEngines")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvaiableSearchEngines()
        {
            _logger.LogTrace("Method GetAvaiableSearchEngines called in SyncController");

            return Ok(await _service.GetAvailableSearchEngines());
        }


        [ServiceFilter(typeof(CacheActionFilter))]
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
