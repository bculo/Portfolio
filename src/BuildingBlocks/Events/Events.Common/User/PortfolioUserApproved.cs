﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.User
{
    public class PortfolioUserApproved
    {
        public string UserName { get; set; }
        public long InternalId { get; set; }
        public Guid ExternalId { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
}