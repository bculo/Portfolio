﻿using Microsoft.Extensions.Options;
using Tracker.Application.Common.Options;
using Tracker.Application.Interfaces;
using Tracker.Core.Enums;

namespace Tracker.Infrastructure.Services
{
    public class FinancialAssetClientFactory : IFinancialAssetClientFactory
    {
        private readonly IServiceProvider _provider;
        private readonly FinancalAssetClientFactoryOptions _options;

        public FinancialAssetClientFactory(IServiceProvider provider,
            IOptionsSnapshot<FinancalAssetClientFactoryOptions> options)
        {
            _provider = provider;
            _options = options.Value;
        }

        public IFinancialAssetClient CreateClient(FinancalAssetType type)
        {
            return type switch
            {
                FinancalAssetType.Crypto => _provider.GetService(typeof(CryptoHttpAssetClient)) as IFinancialAssetClient,
                _ => throw new NotSupportedException($"Financial asset client for type {type} not supported")
            };
        }
    }
}
