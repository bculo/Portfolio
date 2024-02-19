using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Worker.Interfaces
{
    public interface IPriceUpdateJobService
    {
        Task ExecuteUpdate();
    }
}
