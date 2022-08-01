using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.SeedData
{
    public static class CryptoStaticData
    {
        public static List<Core.Entities.Crypto> GetCryptos()
        {
            var result = new List<Core.Entities.Crypto>();

            result.Add(new Core.Entities.Crypto
            {
                Symbol = "BTC",
                Name = "Bitcoin",
                Description = "Bitcoin crypto",
                Logo = "Bitcoin logo",
                Prices = new List<Core.Entities.CryptoPrice>
                {
                    new Core.Entities.CryptoPrice
                    {
                        Price = 21000m,
                    },
                    new Core.Entities.CryptoPrice
                    {
                        Price = 21300m,
                    }
                }
            });

            result.Add(new Core.Entities.Crypto
            {
                Symbol = "ETH",
                Name = "Etherum",
                Description = "Etherum crypto",
                Logo = "Etherum logo",
                Prices = new List<Core.Entities.CryptoPrice>
                {
                    new Core.Entities.CryptoPrice
                    {
                        Price = 1500m,
                    },
                    new Core.Entities.CryptoPrice
                    {
                        Price = 1600m,
                    },
                    new Core.Entities.CryptoPrice
                    {
                        Price = 1530,
                    }
                }
            });

            result.Add(new Core.Entities.Crypto
            {
                Symbol = "ADA",
                Name = "Cardano",
                Description = "Cardano crypto",
                Logo = "Cardano logo",
                Prices = new List<Core.Entities.CryptoPrice>
                {
                    new Core.Entities.CryptoPrice
                    {
                        Price = 0.5m,
                    },
                    new Core.Entities.CryptoPrice
                    {
                        Price =  0.7m,
                    },
                    new Core.Entities.CryptoPrice
                    {
                        Price =  0.6m,
                    },
                    new Core.Entities.CryptoPrice
                    {
                        Price =  0.3m,
                    }
                }
            });

            return result;
        }

        public static string[] GetSupportedCryptoSymbols()
        {
            return new string[] { "TETHA", "TFUEL", "MATIC", "USDT", "USDC", "BNB", "ETH", "ADA", "BTC" };
        }
    }
}
