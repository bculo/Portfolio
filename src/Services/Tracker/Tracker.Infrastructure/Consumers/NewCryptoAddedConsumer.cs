using Events.Common.Crypto;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Application.Common.Models;
using Tracker.Application.Common.Utilities;
using Tracker.Application.Interfaces;
using Tracker.Core.Enums;

namespace Tracker.Infrastructure.Consumers
{
    public class NewCryptoAddedConsumer : BasePriceUpdatedConsumer, IConsumer<NewCryptoAdded>
    {
        public override FinancalAssetType AssetType => FinancalAssetType.Crypto;

        public NewCryptoAddedConsumer(ITrackerCacheService cache)
            : base(cache) { }

        public async Task Consume(ConsumeContext<NewCryptoAdded> context)
        {
            var instance = context.Message;

            var newCacheItem = new FinancialAssetItem
            {
                Symbol = instance.Symbol,
                Price = instance.Price,
                Type = AssetType
            };

            await HandlePriceUpdate(newCacheItem);
        }
    }
}
