using System.Net;
using Crypto.API.Controllers;
using Crypto.Application.Modules.Crypto.Queries.GetMostPopular;
using Crypto.Shared.Builders;
using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Crypto.IntegrationTests.CryptoController;

public class FetchMostPopularEndpointTests(CryptoApiFactory factory) : BaseCryptoEndpointTests(factory)
{
    [Fact]
    public async Task ShouldReturnPopularItems_WhenEndpointIsCalled()
    {
        await Authenticate(UserRole.Admin);

        var cryptoAsset1 = await Fixture.Add(new CryptoEntityBuilder().Build());
        await Fixture.Add(new VisitEntityBuilder().WithCryptoId(cryptoAsset1.Id).Build());
        await Fixture.Add(new VisitEntityBuilder().WithCryptoId(cryptoAsset1.Id).Build());
        
        var cryptoAsset2 = await Fixture.Add(new CryptoEntityBuilder().Build());
        await Fixture.Add(new VisitEntityBuilder().WithCryptoId(cryptoAsset2.Id).Build());
        
        var response = await Client.GetAsync($"{EndpointsConfigurations.CryptoEndpoints.Popular}?Take=50");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.ExtractContentFromResponse<List<GetMostPopularResponse>>();
        responseContent.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}
