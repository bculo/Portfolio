using AutoFixture;
using Crypto.Infrastructure.Information.Models;
using Tests.Common.Extensions;
using WireMock.Server;

namespace Crypto.IntegrationTests.Helpers;

public static class CoinMarketCapClientFacade
{
    public static async Task MockValidResponse(WireMockServer server, Fixture fixture, string symbol)
    {
        var infoResponse = fixture.Build<CoinMarketCapRootResponseDto>().Create();
        var firstItem = infoResponse.Data.Values!.FirstOrDefault();
        firstItem!.Insert(0, fixture.Build<CoinMarketCapInfoDto>()
            .With(x => x.Symbol, symbol)
            .With(x => x.Name, symbol)
            .Create());
        
        await server.ReturnsWithJsonOk(infoResponse, withPath: "/info");
    }
}