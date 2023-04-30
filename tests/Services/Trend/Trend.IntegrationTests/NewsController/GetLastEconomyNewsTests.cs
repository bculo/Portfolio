using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    [Collection("TrendCollection")]
    public class GetLastEconomyNewsTests : BaseTests
    {
        public GetLastEconomyNewsTests(TrendApiFactory factory) : base(factory)
        {
        }
    }
}
