using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IGoogleSyncService _syncService;

        public SyncController(IGoogleSyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet("Sync")]
        public async Task<IActionResult> Sync()
        {
            var dictionary = new Dictionary<ArticleType, IReadOnlyList<string>>
            {
                { ArticleType.Crypto, new List<string> { "bitcoin" } }
            };

            return Ok((await _syncService.Sync(dictionary)).GetInstances());
        }
    }
}
