using System.Net;
using Tests.Common.Interfaces.Text;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Tests.Common.Extensions;

public static class WireMockServerExtensions
{
    public static Task ReturnsBadRequest(this WireMockServer server, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create().WithStatusCode(HttpStatusCode.BadRequest)
            );

        return Task.CompletedTask;
    } 
    
    public static Task ReturnsOk(this WireMockServer server, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create().WithStatusCode(HttpStatusCode.NoContent)
            );

        return Task.CompletedTask;
    } 
    
    public static Task ReturnsWithTextOk(this WireMockServer server, string text, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create()
                    .WithBody(text)
                    .WithStatusCode(HttpStatusCode.OK)
            );

        return Task.CompletedTask;
    } 
    
    public static async Task ReturnsWithTextOk(this WireMockServer server, ITextLoader loader, string withPath = "*")
    {
        var text = await loader.LoadAsync();
        
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create()
                    .WithBody(text)
                    .WithStatusCode(HttpStatusCode.OK)
            );
    } 
    
    public static Task ReturnsWithJsonOk(this WireMockServer server, object instance, string withPath = "*")
    {
        server.Given(Request.Create().WithPath(withPath))
            .RespondWith(
                Response.Create()
                    .WithBodyAsJson(instance)
                    .WithStatusCode(HttpStatusCode.OK)
            );

        return Task.CompletedTask;
    } 
}