using Crypto.Infrastracture.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Crypto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult AppVersion()
        {
            return Ok(Assembly.GetExecutingAssembly().GetName().Version!.ToString());
        }
    }
}
