using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Tracker.API.Endpoints;

[ApiController]
public abstract class TrackerEndpoint : ControllerBase
{
    public IMediator Mediator => GetService<IMediator>();
    public TService GetService<TService>()
        where TService : notnull => HttpContext.RequestServices.GetRequiredService<TService>();
}