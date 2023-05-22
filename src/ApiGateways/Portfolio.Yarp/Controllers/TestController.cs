using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Yarp.Controllers;

[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Test");
    }
}