using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Futures.Contracts;
using Microsoft.AspNetCore.Http;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public record SearchWordAddReqDto
    {
        public string SearchWord { get; init; }
        public int SearchEngine { get; init; }
        public int ContextType { get; init; }
    }
}
