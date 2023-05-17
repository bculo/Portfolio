using Carter;

namespace Mail.API.Modules;

public class TestModule : ICarterModule
{
    private const string MODULE_NAME = "Test";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{MODULE_NAME}", (ILoggerFactory factory) =>
        {
            var logger = factory.CreateLogger("TestModule");
            logger.LogTrace("Method Test called in module TestModule");
            return Results.Ok("Test endpoint");
        }).WithTags(MODULE_NAME);
    }
}

