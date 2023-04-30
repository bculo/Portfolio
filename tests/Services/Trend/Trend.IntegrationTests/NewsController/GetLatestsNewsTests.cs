using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    [Collection("TrendCollection")]
    public class GetLatestsNewsTests : BaseTests
    {
        public GetLatestsNewsTests(TrendApiFactory factory) : base(factory)
        {
        }
    }
}
