using Crypto.Application.Common.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Portfolio.Queries.FetchAll
{
    public class FetchAllPortfoliosDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? StatusId { get; set; }
    }
}
