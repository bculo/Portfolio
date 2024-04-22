using Carter;
using Mail.Application.Features.Template;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mail.API.Modules;

public class TemplateModule : ICarterModule
{
    private const string MODULE_NAME = "Template";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/v1")
            .MapPost($"{MODULE_NAME}/AddTemplate",
                async ([FromBody] AddTemplate.Command request, IMediator mediator, CancellationToken ct) =>
                {
                    await mediator.Send(request);
                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithTags(MODULE_NAME);

        app.MapGroup("/v1")
            .MapGet($"{MODULE_NAME}/GetTemplates",
                async (IMediator mediator, CancellationToken ct) 
                    => Results.Ok(await mediator.Send(new GetTemplates.Query(), ct)))
            .RequireAuthorization()
            .WithTags(MODULE_NAME);

        app.MapGroup("/v1")
            .MapGet($"{MODULE_NAME}/GetTemplatesForCategory",
                async ([AsParameters] GetTemplatesByCategory.Query query, IMediator mediator, CancellationToken ct)
                    => Results.Ok(await mediator.Send(query, ct)))
            .RequireAuthorization()
            .WithTags(MODULE_NAME);

        app.MapGroup("/v1")
            .MapGet($"{MODULE_NAME}/GetSingleTemplate",
                async ([AsParameters] GetSingleTemplate.Query request, IMediator mediator, CancellationToken ct)
                    => Results.Ok(await mediator.Send(request, ct)))
            .RequireAuthorization()
            .WithTags(MODULE_NAME);

        app.MapGroup("/v1")
            .MapPost($"{MODULE_NAME}/DeactivateTemplate",
                async ([FromBody] DeactivateTemplate.Command request, IMediator mediator, CancellationToken ct) =>
                {
                    await mediator.Send(request, ct);
                    return Results.NoContent();
                })
            .RequireAuthorization()
            .WithTags(MODULE_NAME);
    }
}