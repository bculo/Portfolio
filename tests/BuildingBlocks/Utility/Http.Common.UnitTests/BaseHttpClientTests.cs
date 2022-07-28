using Http.Common.UnitTests.Clients;
using HttpUtility.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.Common.UnitTests
{
    internal class BaseHttpClientTests
    {
        [Fact]
        public void Test()
        {
            var client = Build();

            client.
        }


        private BaseHttpClientImplementation Build()
        {
            return new BaseHttpClientImplementation(new HttpClient());
        }

    }
}
