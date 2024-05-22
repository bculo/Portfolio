using Crypto.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Crypto.API.Controllers;

[Authorize]
[ApiController]
public class InfoController : ControllerBase
{
    [HttpGet(EndpointsConfigurations.InfoEndpoints.AssemblyInfo)]
    public IActionResult GetAssemblyVersion()
    {
        return Ok(Assembly.GetExecutingAssembly().GetName().Version!.ToString());
    }
}

