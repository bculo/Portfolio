using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Mock.Common.Data
{
    public class CryptoDataManager : ICryptoDataManager
    {
        private List<Core.Entities.Crypto> _seedData;
        private List<string> _supportedSymbols;

        public CryptoDataManager()
        {
            _seedData = GetDefaultCryptos();
            _supportedSymbols = GetDefaultSupportedCryptoSymbols();
        }

        public List<Core.Entities.Crypto> GetCryptoSeedData()
        {
            return _seedData;
        }

        public List<string> GetSupportedCryptoSymbols()
        {
            return _supportedSymbols;
        }

        public string[] GetSupportedCryptoSymbolsArray()
        {
            return _supportedSymbols.ToArray();
        }

        public void InitData(List<Core.Entities.Crypto>? seedData, List<string>? supportedSymbols)
        {
            _seedData = seedData ?? new List<Core.Entities.Crypto>();

            supportedSymbols = supportedSymbols ?? new List<string>();
            _supportedSymbols = supportedSymbols.Concat(_seedData.Select(i => i.Symbol.ToUpper()))
                        .Distinct()
                        .ToList();
        }

        public bool IsAnySymbolSupported(IEnumerable<string> symbols)
        {
            return symbols.Any(i => _supportedSymbols.Contains(i.ToUpper()));
        }

        public bool IsSymbolSupported(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return false;
            }

            return _supportedSymbols.Contains(symbol.ToUpper());
        }

        private List<Core.Entities.Crypto> GetDefaultCryptos()
        {
            var defaultCryptoList = new List<Core.Entities.Crypto>();

            defaultCryptoList.Add(new Core.Entities.Crypto
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

            defaultCryptoList.Add(new Core.Entities.Crypto
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

            defaultCryptoList.Add(new Core.Entities.Crypto
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

            return defaultCryptoList;
        }

        private List<string> GetDefaultSupportedCryptoSymbols()
        {
            var symbols = new string[] { "TETHA", "TFUEL", "MATIC", "USDT", "USDC", "BNB" };
            var allSupportedSymbols = symbols.Concat(_seedData.Select(i => i.Symbol.ToUpper()))
                .Distinct()
                .ToList();
            return allSupportedSymbols;
        }    
    }
}
