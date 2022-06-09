using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Models.Dtos.Google;

namespace Trend.Application.Interfaces
{
    public interface IGoogleSearchClient
    {
        Task<GoogleSearchEngineResponseDto> Search(string searchDefinition);
    }
}
