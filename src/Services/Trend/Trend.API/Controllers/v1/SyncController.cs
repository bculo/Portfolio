using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.Sync;
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
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;

        public SyncController(ISyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet("Sync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sync()
        {
            await _syncService.ExecuteSync();
            return NoContent();
        }

        [HttpGet("GetSync/{id}")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncStatusDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSync(string id)
        {
            return Ok(await _syncService.GetSync(id));
        }

        [HttpGet("GetSyncStatuses")]
        [OutputCache(PolicyName = "SyncPolicy")]
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
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncStatusWordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusWords(string id)
        {
            return Ok(await _syncService.GetSyncStatusSearchWords(id));
        }
    }
}
