using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Persistence;
using Crypto.UnitTests.Infrastracture.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common;
using Time.Common.Contracts;

namespace Crypto.UnitTests.Infrastracture
{
    public class UnitOfWorkTests
    {
        [Fact]
        public async Task Commit_ShouldAttachTimestamp_WhenExecuted()
        {
            var crypto = Create();
            var work = BuildUnitOfWork();

            await work.CryptoRepository.Add(crypto);
            await work.Commit();

            Assert.NotEqual(crypto.CreatedOn, DateTime.MinValue);
        }

        public IUnitOfWork BuildUnitOfWork()
        {
            IDateTime timeService = new LocalDateTimeService();
            var dbContext = DbInMemoryContextCreator.CreateCryptoContext(timeService);
            return new UnitOfWork(dbContext);
        }

        public Core.Entities.Crypto Create(string symbol = "btc")
        {
            return new Core.Entities.Crypto
            {
                Symbol = symbol,
                Name = symbol,
                Description = "random description",
                Logo = "LOGO_URL",      
            };
        }
    }
}
