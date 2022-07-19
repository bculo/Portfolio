using Dtos.Common.v1.Trend.Article;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IArticleService _service;

        public NewsController(ILogger<NewsController> logger, IArticleService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("GetLatestsNews")]
        [ProducesResponseType(typeof(List<ArticleTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLatestsNews()
        {
            _logger.LogTrace("GetLatestsNews method called");

            return Ok(await _service.GetLatestNews());
        }

        [HttpGet("GetLatestCryptoNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastCryptoNews()
        {
            _logger.LogTrace("GetLastCryptoNews method called");

            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Crypto));
        }

        [HttpGet("GetLatestStockNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastStockNews()
        {
            _logger.LogTrace("GetLastStockNews method called");

            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Stock));
        }

        [HttpGet("GetLatestEtfNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastEtfNews()
        {
            _logger.LogTrace("GetLastEtfNews method called");

            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Etf));
        }

        [HttpGet("GetLatestEconomyNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastEconomyNews()
        {
            _logger.LogTrace("GetLastEconomyNews method called");

            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Economy));
        }
    }
}
