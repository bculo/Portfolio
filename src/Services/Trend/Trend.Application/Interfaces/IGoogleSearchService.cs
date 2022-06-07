using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces
{
    public interface IGoogleSearchService
    {
        Task<string> Search(string searchDefinition);
    }
}
