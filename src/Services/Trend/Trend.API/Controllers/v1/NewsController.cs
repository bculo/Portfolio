using Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Swashbuckle.AspNetCore.Annotations;
using Trend.API.Extensions;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;

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
        
        [HttpGet("GetLatestNews", Name = "GetLatestNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLatestNews(CancellationToken token)
        {
            return Ok(await _service.GetLatest(token));
        }
        
        [HttpPut("Deactivate/{articleId}", Name = "DeactivateArticle")]
        [Authorize(Roles = AppRoles.ADMIN)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deactivate(string articleId, CancellationToken token)
        {
            var request = new DeactivateArticleReqDto(articleId);
            var result = await _service.Deactivate(request, token);
            return result.ToActionResult();
        }
        
        [HttpPut("Activate/{articleId}", Name = "ActivateArticle")]
        [Authorize(Roles = AppRoles.ADMIN)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Activate([FromRoute] string articleId, CancellationToken token)
        {
            var request = new ActivateArticleReqDto(articleId);
            var result = await _service.Activate(request, token);
            return result.ToActionResult();
        }

        [HttpGet("GetLatestCryptoNews", Name = "GetLatestCryptoNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLastCryptoNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestByContext(ContextType.Crypto, token));
        }
        
        [HttpGet("GetLatestStockNews", Name = "GetLatestStockNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLastStockNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestByContext(ContextType.Stock, token));
        }
        
        [HttpGet("GetLatestForexNews", Name = "GetLatestForexNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(List<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLatestForexNews(CancellationToken token)
        {
            return Ok(await _service.GetLatestByContext(ContextType.Forex, token));
        }
        
        [HttpGet("Filter", Name = "FilterNews")]
        [OutputCache(PolicyName = "NewsPolicy")]
        [ProducesResponseType(typeof(PageResponseDto<ArticleResDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery] FilterArticlesReqDto request, CancellationToken token)
        {
            var result = await _service.Filter(request, token);
            return result.ToActionResult();
        }
    }
}
