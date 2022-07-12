using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Event.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAppVersion()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}
