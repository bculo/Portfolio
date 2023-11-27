using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.Sync;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;

namespace Trend.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;

        public SyncController(ISyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet("Sync")]
        [ProducesResponseType(typeof(SyncResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sync()
        {
            return Ok(await _syncService.ExecuteSync());
        }

        [HttpGet("GetSync/{id}")]
        [ProducesResponseType(typeof(List<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSync(string id)
        {
            return Ok(await _syncService.GetSync(id));
        }

        [HttpGet("GetSyncStatuses")]
        [ProducesResponseType(typeof(List<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatuses()
        {
            return Ok(await _syncService.GetSyncStatuses());
        }

        [HttpPost("GetSyncStatusesPage")]
        [ProducesResponseType(typeof(PageResponseDto<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusesPage(PageRequestDto page)
        {
            return Ok(await _syncService.GetSyncStatusesPage(page));
        }

        [HttpGet("GetSyncStatusWords/{id}")]
        [ProducesResponseType(typeof(List<SyncStatusWordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusWords(string id)
        {
            return Ok(await _syncService.GetSyncStatusSearchWords(id));
        }
    }
}
