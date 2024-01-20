using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Yarp.Proxy.Common.Options;

namespace Yarp.Proxy.Controllers;

[ApiController]
[Route("[controller]")]
public class InfoController : ControllerBase
{
    private readonly IOptionsSnapshot<ApplicationOptions> _options;

    public InfoController(IOptionsSnapshot<ApplicationOptions> options)
    {
        _options = options;
    }
    
    [HttpGet("GetReverseProxyName")]
    public IActionResult GetReverseProxyName()
    {
        return Ok(_options.Value.Name);
    }
}