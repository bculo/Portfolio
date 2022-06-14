using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly ILogger<SyncController> _logger;
        private readonly ISyncService _syncService;

        public SyncController(ISyncService syncService, ILogger<SyncController> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        [HttpGet("Sync")]
        [ProducesResponseType(typeof(List<ArticleGroupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sync()
        {
            _logger.LogTrace("Method Sync called in SyncController");

            return Ok((await _syncService.ExecuteGoogleSync()).GetInstances());
        }

        [HttpGet("GetSyncStatuses")]
        [ProducesResponseType(typeof(List<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatuses()
        {
            _logger.LogTrace("Method GetSyncStatuses called in SyncController");

            return Ok(await _syncService.GetSyncStatuses());
        }

        [HttpGet("GetSyncSettingsWords")]
        [ProducesResponseType(typeof(List<SyncSettingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncSettingsWords()
        {
            _logger.LogTrace("Method GetSyncSettingsWords called in SyncController");

            return Ok(await _syncService.GetSyncSettingsWords());
        }

        [HttpGet("GetAvaiableSearchEngines")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvaiableSearchEngines() 
        {
            _logger.LogTrace("Method GetAvaiableSearchEngines called in SyncController");

            return Ok(await _syncService.GetAvailableSearchEngines());
        }

        [HttpGet("GetAvaiableContextTypes")]
        [ProducesResponseType(typeof(List<KeyValueElementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvaiableContextTypes()
        {
            _logger.LogTrace("Method GetAvaiableContextTypes called in SyncController");

            return Ok(await _syncService.GetAvaiableContextTypes());
        }

        [HttpPost("AddNewSearchWord")]
        [ProducesResponseType(typeof(SyncSettingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNewSearchWord([FromBody] SyncSettingCreateDto request)
        {
            _logger.LogTrace("Method AddNewSearchWord called in SyncController");

            return Ok(await _syncService.AddNewSyncSetting(request));
        }

        [HttpDelete("RemoveSearchWord/{id}")]
        [ProducesResponseType(typeof(SyncSettingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveSearchWord(string id)
        {
            _logger.LogTrace("Method RemoveSearchWord called in SyncController");

            await _syncService.RemoveSyncSetting(id);

            return NoContent();
        }
    }
}
