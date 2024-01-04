using Dtos.Common;
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
        public async Task<IActionResult> Sync(CancellationToken token)
        {
            await _syncService.ExecuteSync(token);
            return NoContent();
        }

        [HttpGet("GetSync/{id}")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(SyncStatusResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSync(string id, CancellationToken token)
        {
            return Ok(await _syncService.GetSync(id, token));
        }

        [HttpGet("GetSyncStatuses")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncStatusResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatuses(CancellationToken token)
        {
            return Ok(await _syncService.GetSyncStatuses(token));
        }

        [HttpPost("GetSyncStatusesPage")]
        [OutputCache(PolicyName = "SyncPostPolicy")]
        [ProducesResponseType(typeof(PageResponseDto<SyncStatusResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusesPage(PageRequestDto page, CancellationToken token)
        {
            return Ok(await _syncService.GetSyncStatusesPage(page, token));
        }

        [HttpGet("GetSyncStatusWords/{id}")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncStatusWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusWords(string id, CancellationToken token)
        {
            return Ok(await _syncService.GetSyncStatusSearchWords(id, token));
        }
    }
}
