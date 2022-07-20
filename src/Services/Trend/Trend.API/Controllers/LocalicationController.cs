using Keycloak.Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Trend.Application.Interfaces;
using Trend.Application.Resources;

namespace Trend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LocalicationController : ControllerBase
    {
        private readonly ILanguageService<AppCommon> _language;

        public LocalicationController(ILanguageService<AppCommon> language)
        {
            _language = language;
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var culture = _language.GetCurrentCulture();

            return Ok(_language.Get(id));
        }
    }
}
