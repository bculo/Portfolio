using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;

namespace Crypto.UnitTests.Infrastracture.Common
{
    public static class DbInMemoryContextCreator
    {
        private static DbContextOptions<CryptoDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<CryptoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        public static CryptoDbContext CreateCryptoContext(IDateTime time)
        {
            var dbOptions = GetDbOptions();

            return new CryptoDbContext(dbOptions, time);
        }

        public static void SeedData(IUnitOfWork work)
        {
            var instance1 = new Core.Entities.Crypto { Symbol = "btc", Name = "Vitcoin" };
            var instance2 = new Core.Entities.Crypto { Symbol = "eth", Name = "Etherum" };
            var instance3 = new Core.Entities.Crypto { Symbol = "ada", Name = "Cardano" };

            work.CryptoRepository.Add(instance1);
            work.CryptoRepository.Add(instance2);
            work.CryptoRepository.Add(instance3);

            work.Commit();
        } 
    }
}
