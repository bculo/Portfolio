using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Trend.API.Extensions;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;

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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Sync(CancellationToken token)
        {
            var result = await _syncService.ExecuteSync(token);
            return result.ToActionResult();
        }

        [HttpGet("GetSync/{id}")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(SyncStatusResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSync(string id, CancellationToken token)
        {
            var result = await _syncService.GetSync(id, token);
            return result.ToActionResult();
        }

        [HttpGet("GetSyncStatuses")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncStatusResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSyncStatuses(CancellationToken token)
        {
            return Ok(await _syncService.GetSyncStatuses(token));
        }
        
        [HttpGet("GetSyncStatusWords/{id}")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncStatusWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSyncStatusWords(string id, CancellationToken token)
        {
            var result = await _syncService.GetSyncStatusSearchWords(id, token);
            return result.ToActionResult();
        }
    }
}
