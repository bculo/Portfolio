using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trend.API.Entities;
using Trend.API.Interfaces;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IRepository<Info> _repository;

        public NewsController(IRepository<Info> repository)
        {
            _repository = repository;
        }

        [HttpGet("GetLatestCryptoNews")]
        public async Task<IActionResult> GetLastCryptoNews()
        {
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
            throw new Exception("TEST");
            return Ok("ETF NEWS");
        }
    }
}
