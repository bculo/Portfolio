using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Domain.Entities
{
    public class SyncSetting : IDocument
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool IsActive { get; set; }
        public string SearchWord { get; set; }
        public SearchEngine Engine { get; set; }
        public ContextType Type { get; set; }
    }
}
