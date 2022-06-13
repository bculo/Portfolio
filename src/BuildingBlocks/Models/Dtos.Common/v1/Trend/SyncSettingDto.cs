using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common.v1.Trend
{
    public class SyncSettingDto
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string SearchWord { get; set; }
        public string SearchEngineName { get; set; }
        public int SearchEngineId { get; set; }
    }
}
