using Crypto.Application.Models.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchGroup
{
    public class FetchGroupQuery : PageBaseQuery, IRequest<IEnumerable<FetchGroupResponseDto>>
    {
        public List<string>? Symbols { get; set; }
    }
}
