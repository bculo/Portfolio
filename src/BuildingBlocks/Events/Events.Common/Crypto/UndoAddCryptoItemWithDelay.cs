using Events.Common.Common;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Common.Crypto
{
    public class UndoAddCryptoItemWithDelay
    {
        public Guid TemporaryId { get; set; }
    }
}
