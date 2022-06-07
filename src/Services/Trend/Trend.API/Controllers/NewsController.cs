using Microsoft.AspNetCore.Mvc;
using Trend.Domain.Entities;
using Trend.Domain.Interfaces;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IRepository<Info> _repositry;

        public NewsController(ILogger<NewsController> logger, IRepository<Info> repository)
        {
            _logger = logger;
            _repositry = repository;
        }

        [HttpGet("GetLatestCryptoNews")]
        public async Task<IActionResult> GetLastCryptoNews()
        {
            _logger.LogInformation("CRYPTO NEWS");
            return Ok("CRYPTO NEWS");
        }

        [HttpGet("GetLatestStockNews")]
        public async Task<IActionResult> GetLastStockNews()
        {
            return Ok("STOCKS NEWS");
        }

        [HttpGet("GetLatestEtfNews")]
        public async Task<IActionResult> GetLastEtfNews()
        {
            return Ok("ETF NEWS");
        }
    }
}
