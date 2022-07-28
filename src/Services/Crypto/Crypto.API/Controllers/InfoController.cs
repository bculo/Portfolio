using Crypto.Infrastracture.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Crypto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult AppVersion()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Version!.ToString());
        }
    }
}
