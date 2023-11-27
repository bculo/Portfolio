﻿using Dtos.Common.v1.Trend.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(List<ArticleTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLatestNews()
        {
            return Ok(await _service.GetLatestNews());
        }

        [HttpGet("GetLatestCryptoNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastCryptoNews()
        {
            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Crypto));
        }

        [HttpGet("GetLatestStockNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastStockNews()
        {
            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Stock));
        }

        [HttpGet("GetLatestEtfNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastEtfNews()
        {
            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Etf));
        }

        [HttpGet("GetLatestEconomyNews")]
        [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastEconomyNews()
        {
            return Ok(await _service.GetLatestNews(Domain.Enums.ContextType.Economy));
        }
    }
}
