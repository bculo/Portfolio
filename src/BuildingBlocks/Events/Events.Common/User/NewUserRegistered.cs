using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.User
{
    public class NewUserRegistered
    {
        public string UserName { get; set; } = default!;
        public DateTime Time { get; set; }
    }
}
