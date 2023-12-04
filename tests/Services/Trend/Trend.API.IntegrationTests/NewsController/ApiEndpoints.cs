using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    public static class ApiEndpoints
    {
        public const string GetLatestNews = "/api/v1/news/GetLatestNews";
        public const string GetLatestCryptoNews = "/api/v1/news/GetLatestCryptoNews";
        public const string GetLatestEconomyNews = "/api/v1/news/GetLatestEconomyNews";
        public const string GetLatestEtfNews = "/api/v1/news/GetLatestEtfNews";
        public const string GetLatestStockNews = "/api/v1/news/GetLatestStockNews";
    }
}
