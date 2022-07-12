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
        public async Task Add_Should_Add_New_Instance_When_Add_Action_Commited()
        {
            var crypto = Create();

            var work = BuildUnitOfWork();

            var oldStateCounter = await work.CryptoRepository.Count();

            await work.CryptoRepository.Add(crypto);
            await work.Commit();

            var dbInstance = await work.CryptoRepository.FindSingle(i => i.Symbol == crypto.Symbol);

            Assert.Equal(crypto.Symbol, crypto.Symbol);
            Assert.Equal(crypto.Name, crypto.Name);

            var newStateCounter = await work.CryptoRepository.Count();

            Assert.Equal(oldStateCounter + 1, newStateCounter);
        }

        [Fact]
        public async Task Add_Should_Not_Add_New_Instance_When_Add_Action_Not_Commited()
        {
            var crypto = Create();

            var work = BuildUnitOfWork();

            var oldStateCounter = await work.CryptoRepository.Count();

            await work.CryptoRepository.Add(crypto);

            var newStateCounter = await work.CryptoRepository.Count();

            Assert.Equal(oldStateCounter, newStateCounter);
        }

        [Fact]
        public async Task Remove_Should_Delete_Instance_When_Remove_Action_Commited()
        {
            var work = BuildUnitOfWork();

            DbInMemoryContextCreator.SeedData(work);

            var deleteDbInstance = await work.CryptoRepository.FindSingle(i => i.Symbol == "btc");
            var oldStateCounter = await work.CryptoRepository.Count();

            await work.CryptoRepository.Remove(deleteDbInstance);
            await work.Commit();

            var exist = await work.CryptoRepository.FindSingle(i => i.Symbol == "btc");
            var newStateCounter = await work.CryptoRepository.Count();

            Assert.Null(exist);
            Assert.Equal(oldStateCounter - 1, newStateCounter);
        }

        [Fact]
        public async Task AddRange_Should_Increment_Count_By_2_When_Two_Instances_Added_And_Commited()
        {
            var work = BuildUnitOfWork();

            DbInMemoryContextCreator.SeedData(work);

            var oldStateCounter = await work.CryptoRepository.Count();

            var newInstanecOne = Create("theta");
            var newInstanecTwo = Create("tfuel");

            await work.CryptoRepository.AddRange(new List<Core.Entities.Crypto> { newInstanecOne, newInstanecTwo});
            await work.Commit();

            var newStateCounter = await work.CryptoRepository.Count();

            Assert.Equal(oldStateCounter + 2, newStateCounter);
        }

        [Fact]
        public async Task Remove_Should_Not_Delete_Instance_When_Remove_Action_Not_Commited()
        {
            var work = BuildUnitOfWork();

            DbInMemoryContextCreator.SeedData(work);

            var deleteDbInstance = await work.CryptoRepository.FindSingle(i => i.Symbol == "btc");
            var oldStateCounter = await work.CryptoRepository.Count();

            await work.CryptoRepository.Remove(deleteDbInstance);

            var newStateCounter = await work.CryptoRepository.Count();

            Assert.Equal(oldStateCounter, newStateCounter);
        }

        [Fact]
        public async Task Commit_Should_Attach_Timestamp_When_Commit_Action_Executed()
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
