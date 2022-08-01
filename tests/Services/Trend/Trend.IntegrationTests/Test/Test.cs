using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.Test
{
    public class Test : IClassFixture<TrendApiFactory>
    {
        private readonly TrendApiFactory _factory;

        public Test(TrendApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public void TestV2()
        {
            _factory.CreateClient();

            Assert.True(true);
        }
    }
}
