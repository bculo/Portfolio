using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class AddNewTests : IClassFixture<CryptoApiFactory>, IClassFixture<RabbitMqFixture>
    {
        private readonly CryptoApiFactory _factory;

        public AddNewTests(CryptoApiFactory factory, RabbitMqFixture rabbitMq)
        {
            _factory = factory;

            rabbitMq.ConfigureRabbitMq(_factory);
        }

        [Fact]
        public void Test()
        {

            Assert.True(true);
        }


        [Fact]
        public void Test2()
        {

            Assert.True(true);
        }
    }
}
