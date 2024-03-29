﻿using Auth0.Abstract.Models;
using AutoFixture;
using FluentAssertions;
using Keycloak.Common.Clients;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakUserInfoClientTests
    {
        private const string VALID_ACCESS_TOKEN = "asdsadjksadjksadkjsakdsalkdsakdjsaldkj";

        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task GetUserInfo_ShouldReturnInstance_WhenValidAccessTokenProvided()
        {
            //Arrange
            var accessToken = VALID_ACCESS_TOKEN;
            var client = CreateClient(accessToken);

            //Act
            var response = await client.GetUserInfo(accessToken);

            //Assert
            response.Should().NotBeNull();
            response.UserId.Should().NotBeEmpty();
            response.UserName.Should().NotBeEmpty();
            response.FirstName.Should().NotBeEmpty();
            response.LastName.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUserInfo_ShouldReturnNull_WhenInvalidAccessTokenProvided()
        {
            //Arrange
            var accessToken = "somerandominvalidtoken";
            var client = CreateClient(accessToken);

            //Act
            var response = await client.GetUserInfo(accessToken);

            //Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetUserInfo_ShouldThrowArgumentNullException_WhenNullAccessTokenProvided()
        {
            //Arrange
            string? accessToken = null;
            var client = CreateClient(accessToken);

            //Act and assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetUserInfo(accessToken));
        }


        private KeycloakUserInfoClient CreateClient(string accessToken)
        {
            string authorizationServerUrl = "http://localhost:8080/auth/realms/PortfolioRealm/";

            var handler = new MockHttpMessageHandler();
            if(accessToken == VALID_ACCESS_TOKEN)
            {
                handler.When(HttpMethod.Get, "*").Respond("application/json", JsonConvert.SerializeObject(_fixture.Create<UserInfoResponse>()));
            }
            else
            {
                handler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.BadRequest);
            }

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(handler.ToHttpClient());

            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakUserInfoOptions
            {
                AuthorizationServerUrl = authorizationServerUrl,
            });

            var logger = Mock.Of<ILogger<KeycloakUserInfoClient>>();

            return new KeycloakUserInfoClient(options, logger, factoryMock.Object);
        }
    }
}
