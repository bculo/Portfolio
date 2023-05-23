using Crypto.gRPC.Protos.v1;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Interfaces;
using Tracker.Core.Enums;

namespace Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IFinancialAssetClientFactory _factory;
    
    public TestController(IFinancialAssetClientFactory factory)
    {
        _factory = factory;
    }

    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        var client = _factory.CreateClient(FinancalAssetType.Crypto);
        var clientResponse = await client.FetchAsset("BTC");
        return Ok(clientResponse);
    }
}