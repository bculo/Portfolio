﻿using Crypto.Application.Modules.Crypto.Queries.FetchAll;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using FluentAssertions;
using Newtonsoft.Json;
using Tests.Common.Extensions;
using Tests.Common.Utilities;

namespace Crypto.IntegrationTests.CryptoController
{
    public class FetchAllTests : BaseTests
    {
        public FetchAllTests(CryptoApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAll_ShouldReturnStatusOk_WhenExecutedSuccessfully()
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new FetchAllQuery { });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.GetAsync(ApiEndpoint.CRYPTO_FETCH_ALL);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var instances = JsonConvert.DeserializeObject<List<FetchAllResponseDto>>(jsonResponse);
            instances.Should().AllBeOfType<FetchAllResponseDto>();
        }
    }
}
