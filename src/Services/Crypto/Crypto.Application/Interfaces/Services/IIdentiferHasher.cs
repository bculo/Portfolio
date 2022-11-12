using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Interfaces.Services
{
    public interface IIdentiferHasher
    {
        string Encode(long value);
        long Decode(string value);
    }
}
