using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces.Models.Google;

namespace Trend.Application.Interfaces
{
    public interface IGoogleSearchClient
    {
        Task<GoogleSearchEngineResponseDto> Search(string searchDefinition);
    }
}
