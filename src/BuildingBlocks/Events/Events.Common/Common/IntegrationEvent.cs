using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Common
{
    public abstract class IntegrationEvent
    {
        public DateTime CreatedOn { get; set; }
    }
}
