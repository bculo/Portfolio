using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Interfaces;
using Tracker.Core.Enums;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IFinancialAssetClientFactory _factory;
    
    public TestController(IFinancialAssetClientFactory factory)
    {
        _factory = factory;
    }

    [HttpGet("Test/{symbol}")]
    public async Task<IActionResult> Test(string symbol)
    {
        var client = _factory.CreateClient(FinancalAssetType.Crypto);
        var clientResponse = await client.FetchAsset(symbol);
        return Ok(clientResponse);
    }
}