using System.Net;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using NSubstitute;
using RichardSzalay.MockHttp;
using Trend.Application.Clients;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces.Models.Google;

namespace Trend.UnitTests.Application;

public class GoogleSearchClientTests
{
    private readonly Fixture _fixture = new();
    private readonly IOptions<GoogleSearchOptions> _options;
    private readonly IHttpClientFactory _factory = Substitute.For<IHttpClientFactory>();
    private readonly ILogger<GoogleSearchClient> _logger = Substitute.For<ILogger<GoogleSearchClient>>();

    public GoogleSearchClientTests()
    {
        _options = Options.Create(new GoogleSearchOptions()
        {
            Uri = "https://trend.fake.testing.com",
            ApiKey = "ApiKey",
            SearchEngineId = Guid.NewGuid().ToString()
        });
    }
    
    [Fact]
    public async Task Search_ShouldReturnResult_WhenOkResponseReceived()
    {
        var googleResponse = _fixture.Create<GoogleSearchEngineResponseDto>();
        var mock = new MockHttpMessageHandler();
        mock.When("*")
            .Respond("application/json", JsonSerializer.Serialize(googleResponse));
        
        _factory.CreateClient().Returns(mock.ToHttpClient());
        
        var googleClient = new GoogleSearchClient(_factory, _options, _logger);

        var response = await googleClient.Search("BTC");

        response.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task Search_ShouldReturnNull_WhenNonOkResponseReceived(HttpStatusCode statusCode)
    {
        var googleResponse = _fixture.Create<GoogleSearchEngineResponseDto>();
        var mock = new MockHttpMessageHandler();
        mock.When("*")
            .Respond(statusCode);
        
        _factory.CreateClient().Returns(mock.ToHttpClient());
        
        var googleClient = new GoogleSearchClient(_factory, _options, _logger);

        var response = await googleClient.Search("BTC");

        response.Should().BeNull();
    }
}