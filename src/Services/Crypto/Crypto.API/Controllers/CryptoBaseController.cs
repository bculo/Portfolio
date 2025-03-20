using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.API.Controllers;

[Authorize]
[ApiController]
public class CryptoBaseController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>();
}