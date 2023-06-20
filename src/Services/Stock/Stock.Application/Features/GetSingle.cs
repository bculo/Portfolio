﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Stock.Application.Interfaces;
using Stock.Application.Resources.Shared;
using Stock.Core.Exceptions;
using System.Text.RegularExpressions;

namespace Stock.Application.Features
{
    /// <summary>
    /// Fetch single stock item for given symbol with a price tag
    /// </summary>
    public static class GetSingle
    {
        public record Query : IRequest<Response>
        {
            public string Symbol { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator(IStringLocalizer<ValidationShared> localizer)
            {
                RuleFor(i => i.Symbol)
                    .Matches(new Regex("^[a-zA-Z]{1,10}$",
                        RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1)))
                    .WithMessage(localizer.GetString("Symbol pattern not valid"))
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IStockRepository _repo;
            private readonly IStringLocalizer<GetSingleLocale> _localizer;

            public Handler(IStockRepository repo, IStringLocalizer<GetSingleLocale> localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await _repo.GetCurrentPrice(request.Symbol);
                if (item is null)
                {
                    string excMessage = string.Format(_localizer.GetString("Symbol not found"), request.Symbol);
                    throw new StockCoreNotFoundException(excMessage);
                }

                return new Response
                {
                    Id = item.Id,
                    Symbol = item.Symbol,
                    Price = item.Price
                };
            }
        }

        public class Response
        {
            public long Id { get; set; }
            public string Symbol { get; set; }
            public decimal Price { get; set; }
        }
    }

    public class GetSingleLocale { }
}
