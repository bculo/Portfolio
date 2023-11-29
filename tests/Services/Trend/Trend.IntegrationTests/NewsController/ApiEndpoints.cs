using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    public static class ApiEndpoints
    {
        public const string LATEST_NEWS = "/api/v1/news/GetLatestNews";
        public const string LATEST_CRYPTO_NEWS = "/api/v1/news/GetLatestCryptoNews";
        public const string LATEST_ECONOMY_NEWS = "/api/v1/news/GetLatestEconomyNews";
        public const string LATEST_ETF_NEWS = "/api/v1/news/GetLatestEtfNews";
        public const string LATEST_STOCK_NEWS = "/api/v1/news/GetLatestStockNews";
    }
}
