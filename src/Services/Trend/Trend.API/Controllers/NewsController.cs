using Microsoft.AspNetCore.Mvc;
using Trend.Application.Interfaces;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IArticleService _service;

        public NewsController(ILogger<NewsController> logger, IArticleService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("GetLatestCryptoNews")]
        public async Task<IActionResult> GetLastCryptoNews()
        {
            _logger.LogTrace("GetLastCryptoNews method called");

            var response = await _service.GetLatestNews(Domain.Enums.ArticleType.Crypto);

            return Ok(response);
        }

        [HttpGet("GetLatestStockNews")]
        public async Task<IActionResult> GetLastStockNews()
        {
            _logger.LogTrace("GetLastStockNews method called");

            var response = await _service.GetLatestNews(Domain.Enums.ArticleType.Stock);

            return Ok(response);
        }

        [HttpGet("GetLatestEtfNews")]
        public async Task<IActionResult> GetLastEtfNews()
        {
            _logger.LogTrace("GetLastEtfNews method called");

            var response = await _service.GetLatestNews(Domain.Enums.ArticleType.Etf);

            return Ok(response);
        }
    }
}
