using System.Net;
using Crypto.Application.Modules.Crypto.Queries.GetMostPopular;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Utilities;

namespace Crypto.IntegrationTests.CryptoController;

public class GetMostPopularTests : BaseTests
{
    public GetMostPopularTests(CryptoApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAsync_ShouldReturnStatusOk_WhenValidEndpointInvoked()
    {
        //Arrange
        var model = new GetMostPopularQuery { Take = 2 };
        var request = HttpClientUtilities.PrepareJsonRequest(model);
        _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);
        
        //Act
        var response = await _client.PostAsync(ApiEndpoint.CRYPTO_MOST_POPULAR, request);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}