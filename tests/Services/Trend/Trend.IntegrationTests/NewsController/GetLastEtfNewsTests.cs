using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    [Collection("TrendCollection")]
    public class GetLastEtfNewsTests
    {
        public GetLastEtfNewsTests(TrendApiFactory factory) : base(factory)
        {
        }
    }
}
