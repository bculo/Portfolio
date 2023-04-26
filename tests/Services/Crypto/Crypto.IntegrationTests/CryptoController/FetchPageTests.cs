using Crypto.Application.Modules.Crypto.Queries.FetchPage;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using FluentAssertions;

namespace Crypto.IntegrationTests.CryptoController
{
    [Collection("CryptoCollection")]
    public class FetchPageTests : BaseTests
    {
        public FetchPageTests(CryptoApiFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(-1, 5)]
        [InlineData(2, -1)]
        [InlineData(-10, -1)]
        [InlineData(0, 0)]
        public async Task GetAsync_ShouldReturnStatusBadRequest_WhenInvalidPageArgumentsProvided(int page, int take)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new FetchPageQuery
            {
                Page = page,
                Take = take
            });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_FETCH_PAGE, content);

            //Arrange
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(3, 10)]
        public async Task GetAsync_ShouldReturnStatusOk_WhenValidPageArgumentsProvided(int page, int take)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new FetchPageQuery
            {
                Page = page,
                Take = take
            });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_FETCH_PAGE, content);

            //Arrange
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
