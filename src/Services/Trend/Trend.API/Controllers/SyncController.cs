using Dtos.Common.v1.Trend;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;

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

        [HttpGet("GetSyncStatusWords/{id}")]
        [ProducesResponseType(typeof(List<SyncStatusWordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusWords(string id)
        {
            _logger.LogTrace("Method GetSyncStatusWords called in SyncController");

            return Ok(await _syncService.GetSyncStatusSearchWords(id));
        }
    }
}
