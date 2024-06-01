using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Trend.API.Extensions;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;

namespace Trend.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SyncController(ISyncService syncService) : ControllerBase
    {
        [HttpGet("Sync", Name = "Sync")]
        [Authorize(Roles = AppRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Sync(CancellationToken token)
        {
            var result = await syncService.ExecuteSync(token);
            return result.ToActionResult();
        }

        [HttpGet("GetSync/{id}", Name = "GetSync")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(SyncStatusResDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSync(string id, CancellationToken token)
        {
            var request = new GetSyncStatusReqDto(id);
            var result = await syncService.Get(request, token);
            return result.ToActionResult();
        }

        [HttpGet("GetSyncStatuses", Name = "GetSyncStatuses")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncStatusResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatuses(CancellationToken token)
        {
            return Ok(await syncService.GetAll(token));
        }
        
        [HttpGet("GetSyncStatusWords/{id}", Name = "SyncPolicy")]
        [OutputCache(PolicyName = "SyncPolicy")]
        [ProducesResponseType(typeof(List<SyncSearchWordResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSyncStatusWords(SyncSearchWordsReqDto request, CancellationToken token)
        {
            var result = await syncService.GetSyncSearchWords(request, token);
            return result.ToActionResult();
        }
    }
}
