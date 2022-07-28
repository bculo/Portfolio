using HttpUtility.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.Common.UnitTests.Clients
{
    public class BaseHttpClientImplementation : BaseHttpClient
    {
        public BaseHttpClientImplementation(HttpClient client) : base(client)
        {

        }
    }
}
