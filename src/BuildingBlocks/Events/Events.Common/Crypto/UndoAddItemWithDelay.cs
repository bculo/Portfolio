﻿using Events.Common.Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class UndoAddItemWithDelay
    {
        public Guid CorrelationId { get; set; }
    }
}
