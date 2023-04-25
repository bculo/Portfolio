using Crypto.Infrastracture.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Crypto.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet("AssemblyVersion")]
        public IActionResult GetAssemblyVersion()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Version!.ToString());
        }
    }
}
