using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Options
{
    public class SyncBackgroundServiceOptions
    {
        public int SleepTimeMiliseconds { get; set; }
        public int TimeSpanBetweenSyncsHours { get; set; }
    }
}
