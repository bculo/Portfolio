using Dtos.Common.v1.Trend.Article;
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
    public class NewsController : ControllerBase
    {
        private readonly IArticleService _service;

        public NewsController(IArticleService service)
        {
            _service = service;
        }
        
        [HttpGet("GetLatestNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLatestNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNews(token));
        }

        [HttpGet("GetLatestCryptoNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastCryptoNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNews(token));
        }
        
        [HttpGet("GetLatestStockNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastStockNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Stock, token));
        }
        
        [HttpGet("GetLatestEtfNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastEtfNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Etf, token));
        }
        
        [HttpGet("GetLatestEconomyNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastEconomyNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Economy, token));
        }
    }
}
