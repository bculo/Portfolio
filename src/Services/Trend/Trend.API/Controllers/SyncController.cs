using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.Article;
using Dtos.Common.v1.Trend.Sync;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Action;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [ProducesResponseType(typeof(GoogleSyncResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sync()
        {
            _logger.LogTrace("Method Sync called in SyncController");

            return Ok(await _syncService.ExecuteGoogleSync());
        }

        [HttpGet("GetSync/{id}")]
        [ProducesResponseType(typeof(List<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSync(string id)
        {
            _logger.LogTrace("Method GetSync called in SyncController");

            return Ok(await _syncService.GetSync(id));
        }

        [HttpGet("GetSyncStatuses")]
        [ProducesResponseType(typeof(List<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatuses()
        {
            _logger.LogTrace("Method GetSyncStatuses called in SyncController");

            return Ok(await _syncService.GetSyncStatuses());
        }

        [HttpPost("GetSyncStatusesPage")]
        [ProducesResponseType(typeof(PageResponseDto<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusesPage(PageRequestDto page)
        {
            _logger.LogTrace("Method GetSyncStatuses called in SyncController");

            return Ok(await _syncService.GetSyncStatusesPage(page));
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
