using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Features.Favorite;

namespace Tracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("AddFavorite")]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavorite.Command command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}