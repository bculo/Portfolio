using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory;
using Crypto.Application.Modules.Crypto.Queries.GetMostPopular;
using Crypto.Shared.Builders;
using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController;

public class FetchPriceHistoryEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnPopularItems_WhenEndpointIsCalled()
    {
        await Authenticate(UserRole.Admin);

        var crypto = await Fixture.Add(new CryptoEntityBuilder().Build());
        
        await Fixture.AddPrice(new CryptoPriceEntityBuilder()
            .WithTimestamp(DateTimeOffset.UtcNow)
            .WithCryptoItemId(crypto.Id)
            .Build());
        
        await Fixture.AddPrice(new CryptoPriceEntityBuilder()
            .WithTimestamp(DateTimeOffset.UtcNow.AddMinutes(-10))
            .WithCryptoItemId(crypto.Id)
            .Build());
        
        await Fixture.AddPrice(new CryptoPriceEntityBuilder()
            .WithTimestamp(DateTimeOffset.UtcNow.AddMinutes(-20))
            .WithCryptoItemId(crypto.Id)
            .Build());
        
        await Fixture.AddPrice(new CryptoPriceEntityBuilder()
            .WithTimestamp(DateTimeOffset.UtcNow.AddMinutes(-30))
            .WithCryptoItemId(crypto.Id)
            .Build());
        
        var response = await Client.GetAsync(EndpointsConfigurations.CryptoEndpoints.BuildHistoryUrl(crypto.Id));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseContent = await response.ExtractContentFromResponse<List<PriceHistoryDto>>();
        responseContent.Count.Should().Be(2);
    }
}
