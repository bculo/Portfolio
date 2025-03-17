using System.Net;
using AutoFixture;
using Crypto.Infrastructure.Information.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Crypto.IntegrationTests.Helpers;

public static class CoinMarketCapClientFacade
{
    public static void MockValidResponse(WireMockServer server, Fixture fixture, string symbol)
    {
        var infoResponse = fixture.Build<CoinMarketCapRootResponseDto>().Create();
        
        var firstItem = infoResponse.Data.Values!.FirstOrDefault();
        firstItem!.Insert(0, fixture.Build<CoinMarketCapInfoDto>()
            .With(x => x.Symbol, symbol)
            .With(x => x.Name, symbol)
            .Create());
        
        server.Given(Request.Create()
                .WithPath("/info")
                .WithParam("symbol", symbol))
            .RespondWith(Response.Create()
                .WithBodyAsJson(infoResponse)
                .WithStatusCode(HttpStatusCode.OK));
    }
}