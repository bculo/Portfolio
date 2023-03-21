using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.GetMostPopular
{
    public class GetMostPopularQuery : IRequest<List<GetMostPopularResponse>>
    {
        public int Take { get; set; }
    }
}
