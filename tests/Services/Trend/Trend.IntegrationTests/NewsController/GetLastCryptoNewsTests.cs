using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    [Collection("TrendCollection")]
    public class GetLastCryptoNewsTests : BaseTests
    {
        public GetLastCryptoNewsTests(TrendApiFactory factory) : base(factory)
        {
        }
    }
}
