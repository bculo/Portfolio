using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using Trend.API.Filters.Models;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;

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
        [SwaggerOperation(OperationId = "GetLatestNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLatestNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNews(token));
        }
        
        [HttpGet("Deactivate/{articleId}")]
        [SwaggerOperation(OperationId = "Deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deactivate(string articleId, CancellationToken token)
        {
            await _service.Deactivate(articleId, token);
            return NoContent();
        }
        
        [HttpGet("Activate/{articleId}")]
        [SwaggerOperation(OperationId = "Activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Activate(string articleId, CancellationToken token)
        {
            await _service.Deactivate(articleId, token);
            return NoContent();
        }

        [HttpGet("GetLatestCryptoNews")]
        [SwaggerOperation(OperationId = "GetLatestCryptoNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastCryptoNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNewsByContextType(Domain.Enums.ContextType.Crypto, token));
        }
        
        [HttpGet("GetLatestStockNews")]
        [SwaggerOperation(OperationId = "GetLatestStockNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastStockNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNewsByContextType(Domain.Enums.ContextType.Stock, token));
        }
        
        [HttpGet("GetLatestForexNews")]
        [SwaggerOperation(OperationId = "GetLatestForexNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLatestForexNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestNewsByContextType(Domain.Enums.ContextType.Forex, token));
        }
    }
}
