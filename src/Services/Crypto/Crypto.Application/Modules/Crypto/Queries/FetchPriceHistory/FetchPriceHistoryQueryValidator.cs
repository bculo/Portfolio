﻿using Crypto.Application.Common.Constants;
using FluentValidation;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPriceHistory
{
    public class FetchPriceHistoryQueryValidator : AbstractValidator<FetchPriceHistoryQuery>
    {
        public FetchPriceHistoryQueryValidator()
        {

        }   
    }
}
