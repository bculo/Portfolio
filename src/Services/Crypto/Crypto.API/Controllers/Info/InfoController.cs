using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers.Info;

[ApiExplorerSettings(GroupName = EndpointsConfigurations.InfoEndpoints.GroupTag)]
public class InfoController : CryptoBaseController
{
    [HttpGet(EndpointsConfigurations.InfoEndpoints.AssemblyInfo)]
    public IActionResult GetAssemblyVersion()
    {
        return Ok(Assembly.GetExecutingAssembly().GetName().Version!.ToString());
    }
}

