using AutoFixture;
using Crypto.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Time.Common;
using Time.Common.Contracts;

namespace Crypto.UnitTests.Infrastracture
{
    public class UnitOfWorkTests
    {
        private readonly UnitOfWork _work;

        public UnitOfWorkTests()
        {
            IDateTime timeService = new LocalDateTimeService();

            var dbOptions = new DbContextOptionsBuilder<CryptoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new CryptoDbContext(dbOptions, timeService);

            _work = new UnitOfWork(dbContext, null);
        }

        [Fact]
        public async Task Commit_ShouldAttachTimestamp_WhenExecuted()
        {
            var crypto = new Core.Entities.Crypto
            {
                Description = "Description",
                Symbol = "BTC",
                Name = "Bitcoin",
                Logo = "Btc Logo Path"
            };
            await _work.CryptoRepository.Add(crypto);

            await _work.Commit();

            Assert.NotEqual(crypto.CreatedOn, DateTime.MinValue);
        }
    }
}
