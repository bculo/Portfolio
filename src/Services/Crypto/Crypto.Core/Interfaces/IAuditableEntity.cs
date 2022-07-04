using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime ModifiedOn { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
