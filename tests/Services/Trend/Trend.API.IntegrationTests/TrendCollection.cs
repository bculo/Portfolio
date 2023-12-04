using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests
{
    [CollectionDefinition("TrendCollection")]
    public class TrendCollection : ICollectionFixture<TrendApiFactory>
    {
    }
}
