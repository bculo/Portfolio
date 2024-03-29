﻿using Crypto.Application.Models.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQueryValidator : AbstractValidator<FetchPageQuery>
    {
        public FetchPageQueryValidator()
        {
            RuleFor(i => i.Take).GreaterThan(0);
            RuleFor(i => i.Page).GreaterThan(0);
        }
    }
}
