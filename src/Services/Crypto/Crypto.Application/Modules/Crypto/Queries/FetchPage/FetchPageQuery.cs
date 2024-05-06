using Crypto.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchPage
{
    public class FetchPageQuery : PageBaseQuery, IRequest<IEnumerable<FetchPageResponseDto>>
    {
        public string? Symbol { get; set; }
    }
}
